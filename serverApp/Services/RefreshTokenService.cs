using System.Security.Cryptography.X509Certificates;
using Dapper;

public class RefreshTokenService
{
    public RefreshTokenService(ConnectionFactory factory,
     QueryCreaterService queryCreater, ILogger<UserService> logger)
    {
        _factory = factory;
        _queryCreater = queryCreater;
        _logger = logger;
    }
    private readonly ILogger<UserService> _logger;
    private readonly ConnectionFactory _factory;
    private readonly QueryCreaterService _queryCreater;


    public async Task<bool> Add(Guid userId, string refreshToken)
    {
        using var dbCon = _factory.Create();
        var sqlQuery = "INSERT INTO tokens  (user_id, refresh_token) VALUES (@userIdParam, @refreshParam)";

        _logger.LogDebug(sqlQuery);

        return await dbCon.ExecuteAsync(sqlQuery, AddOrUpdateParams(userId, refreshToken)) >= 0;
    }
    public async Task<bool> UpdateToken(Guid userId, string refreshToken)
    {
        using var dbCon = _factory.Create();
        var sqlQuery = "UPDATE tokens SET refresh_token = @refreshParam WHERE user_id = @userIdParam";

        _logger.LogDebug(sqlQuery);

        return await dbCon.ExecuteAsync(sqlQuery, AddOrUpdateParams(userId, refreshToken)) >= 0;
    }
    public async Task<UserEntity?> GetUser(string refreshToken)
    {
        using var dbCon = _factory.Create();

        string sqlQuery = @$"SELECT * FROM tokens WHERE refresh_token = @tokenParam ";

        _logger.LogDebug(sqlQuery);

        var users = await dbCon.QueryAsync<UserEntity>(sqlQuery, new { tokenParam = refreshToken });
        return users.FirstOrDefault();
    }
    private object AddOrUpdateParams(Guid guid, string refreshToken)
    {
        return new
        {
            userIdParam = guid.ToString(),
            refreshParam = refreshToken
        };
    }


    // public bool RefreshTokenVerify(string refresh, Guid userId);
    // {
    //     // using var con = _factory.Create();
    //     // var sqlQuery = "";


    //     // Тут еще нужно будет токен прочитать чтобы юзера взять, по нему найти сам токен, и проверить, совпадают ли 
    //     // var findToken = await con.QueryAsync<string>
    //     // if (findToken is not null)
    //     //     return true;

    //     // return false;
    // }
}