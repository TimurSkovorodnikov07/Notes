using System.Security.Cryptography.X509Certificates;
using Dapper;

public class RefreshTokenService
{
    public RefreshTokenService(ConnectionFactory factory,
     QueryCreaterService queryCreater, ILogger<UserService> logger,
     JwtService jwtService)
    {
        _factory = factory;
        _queryCreater = queryCreater;
        _logger = logger;
        _jwtService = jwtService;
    }
    private readonly ILogger<UserService> _logger;
    private readonly ConnectionFactory _factory;
    private readonly QueryCreaterService _queryCreater;
    private readonly JwtService _jwtService;


    public async Task<bool> Add(Guid userId, string refreshToken)
    {
        using var dbCon = _factory.Create();
        var sqlQuery = "INSERT INTO tokens (user_id, refresh_token) VALUES (@userIdParam, @refreshParam) ;";

        _logger.LogDebug(sqlQuery);

        return await dbCon.ExecuteAsync(sqlQuery, AddOrUpdateParams(userId, refreshToken)) >= 0;
    }
    public async Task<bool> UpdateToken(Guid userId, string refreshToken)
    {
        using var dbCon = _factory.Create();
        var sqlQuery = "UPDATE tokens SET refresh_token = @refreshParam WHERE user_id = @userIdParam ;";

        _logger.LogDebug(sqlQuery);

        return await dbCon.ExecuteAsync(sqlQuery, AddOrUpdateParams(userId, refreshToken)) >= 0;
    }
    public async Task<Guid?> GetRefresh(Guid userId)
    {
        using var dbCon = _factory.Create();

        string sqlQuery = @$"SELECT user_id AS {nameof(RefreshGetResponse.UserId)}, refresh_token AS {nameof(RefreshGetResponse.RefreshToken)}"
            + $" FROM tokens WHERE user_id = @userIdParam ;";

        _logger.LogDebug(sqlQuery);

        var users = await dbCon.QueryAsync<RefreshGetResponse>(sqlQuery, new { userIdParam = userId.ToString() });
        var user = users.FirstOrDefault();

        return user is not null & user?.UserId is not null && user?.UserId.ToString() is not ""
            ? user.UserId
            : null;
    }
    private object AddOrUpdateParams(Guid guid, string refreshToken)
    {
        return new
        {
            userIdParam = guid.ToString(),
            refreshParam = refreshToken
        };
    }

    public async Task<Guid?> RefreshTokenVerifyAndGetUserId(string refresh)
    {
        using var dbCon = _factory.Create();

        string sqlQuery = @$"SELECT user_id AS {nameof(RefreshGetResponse.UserId)}, refresh_token AS {nameof(RefreshGetResponse.RefreshToken)}"
            + $" FROM tokens WHERE refresh_token = @tokenParam ;";

        _logger.LogDebug(sqlQuery);

        var resList = await dbCon.QueryAsync<RefreshGetResponse>(sqlQuery, new { tokenParam = refresh });
        var res = resList.FirstOrDefault();

        Guid? userId = res is not null & res?.UserId is not null && res?.UserId.ToString() is not ""
            ? res.UserId
            : null;

        return userId is not null
            && _jwtService.RefreshTokenIsValid(refresh, userId.Value)
            ? userId : null;
    }
}