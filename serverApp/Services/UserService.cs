using System.Data;
using System.Data.Common;
using Dapper;

public class UserService
{
    public UserService(ConnectionFactory factory,
     QueryCreaterService queryCreater, ILogger<UserService> logger)
    {
        _factory = factory;
        _queryCreater = queryCreater;
        _logger = logger;
    }
    private readonly ILogger<UserService> _logger;
    private readonly ConnectionFactory _factory;
    private readonly QueryCreaterService _queryCreater;

    public async Task<bool> Add(UserEntity user)
    {
        using var dbCon = _factory.Create();
        var queryPattern = AddOrUpdateSqlQueryPattern();
        var sqlQuery = _queryCreater.AddQuery(queryPattern.Item1, queryPattern.Item2);

        _logger.LogDebug(sqlQuery);

        return await dbCon.ExecuteAsync(sqlQuery, ParamsForAddOrUpdateQuery(user)) >= 0;
    }



    public async Task<UserEntity?> GetUser(Guid guid)
    {
        var users = await GetUsers("id", guid.ToString());
        _logger.LogCritical(users.Count().ToString());

        return users.FirstOrDefault();
    }
    public async Task<IEnumerable<UserEntity>> GetUsersByEmail(string email)
    {
        return await GetUsers("email", email);
    }

    public async Task<IEnumerable<UserEntity>> GetUsers(string filterBy, string value)
    {
        using var dbCon = _factory.Create();

        string sqlQuery = @$"SELECT id AS {nameof(UserEntity.Id)}, " +
            $"name AS {nameof(UserEntity.Name)}, " +
            $"email AS {nameof(UserEntity.Email)}, " +
            $"email_verify AS {nameof(UserEntity.EmailVerify)}, " +
            $"password_hash AS {nameof(UserEntity.PasswordHash)} " +
            $"FROM users " +
            $"WHERE {filterBy} = @valueParam; ";
        //Парраметры выглядят как @param

        _logger.LogDebug(sqlQuery);

        //Dapper и Npgsql должны корректно обрабатывать тип UUID,
        //и вам не нужно использовать дополнительные конвертеры для работы с Guid
        //Это пиздец....СКОлько времени я блять убил...s

        var list = await dbCon.QueryAsync<UserEntity>(sqlQuery, new { valueParam = value });

        foreach (var user in list)
        {
            _logger.LogCritical(user.Name.ToString());
        }
        return list;
        //https://habr.com/ru/articles/665836/
        //https://stackoverflow.com/questions/36325228/comparing-query-and-execute-in-dapper
    }






    public async Task<bool> EmailVerUpdate(Guid guid, bool newValue)
    {
        using var dbCon = _factory.Create();

        var sqlQuery = "UPDATE users SET email_verify = @newValueParam WHERE id = @guidStringParam";

        _logger.LogDebug(sqlQuery);

        return await dbCon.ExecuteAsync(sqlQuery, new
        {
            newValueParam = newValue,
            guidStringParam = guid.ToString()
        }) >= 0;
    }
    public async Task<bool> Update(UserEntity user)
    {
        using var dbCon = _factory.Create();

        var queryPattern = AddOrUpdateSqlQueryPattern();
        var sqlQuery = _queryCreater.UpdateQuery(queryPattern.Item1, queryPattern.Item2);

        _logger.LogDebug(sqlQuery);

        return await dbCon
            .ExecuteAsync(sqlQuery,
            ParamsForAddOrUpdateQuery(user)) >= 0;
    }
    public async Task<bool> Remove(Guid guid)
    {
        using var dbCon = _factory.Create();
        string sqlQuery = @$"DELETE FROM users WHERE id = @filterId ;";//Парраметры выглядят как @param

        _logger.LogDebug(sqlQuery);

        return await dbCon.ExecuteAsync(sqlQuery, new { filterId = guid.ToString() }) >= 0;
    }



    private (string, string[]) AddOrUpdateSqlQueryPattern()
    {
        return ("users", new string[] { "id", "name", "email", "emailVer", "pasHash" });
    }
    private object ParamsForAddOrUpdateQuery(UserEntity user)
    {
        return new
        {
            id = user.Id.ToString(),
            name = user.Name,
            email = user.Email,
            emailVer = user.EmailVerify,
            pasHash = user.PasswordHash
        };
    }
}