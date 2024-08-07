using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
public class JwtService
{
    private readonly JwtOptions _options;
    private readonly ILogger<JwtService> _logger;
    private readonly UserService _userService;
    public JwtService(IOptions<JwtOptions> options,
     ILogger<JwtService> logger, UserService userService)
    {
        _options = options.Value;
        _logger = logger;
        _userService = userService;
    }

    public string AccessTokenCreate(UserEntity user)
    {
        //Алгоритмы которые поддерживаються:
        //https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/Supported-Algorithms
        //Если нужен семетричный юзаешь только SymmetricSecurityKey и симетричный алг, с остальными также.
        //SecurityAlgorithms тут название алг в виде значений конст
        var signingCredentials = new SigningCredentials(
           _options.GetAccessSymmetricSecurityKey(), _options.AlgorithmForAccessToken);
        //бЛять Я даун нахуй. ПРоебал 2-3 часа не понимаю почему в NoteController User = null 
        //Дело было в том что я блять юзал HS512 вместо HS256, я честно не ебу за криптографию, на нее мне насрать, но щас я понял что лучше почитать бы пару статей на тему алгоритмов, 
        //ПРоблемы не было бы если бы я шарил за их различия, да и с ассиметричным refresh token-ом были проблемы, потому стоит.

        var claims = new List<Claim>
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("userName", user.Name.ToString()),
            new Claim("userEmail", user.Email.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            claims: claims,
            notBefore: DateTime.UtcNow/*notBefore - начиная с какого времени токен может быть валидным*/,
            expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenExpiresMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public string RefreshTokenCreate(UserEntity user)
    {
        var signingCredentials = new SigningCredentials(
           _options.GetRefreshAsymmetricSecurityKey(), _options.AlgorithmForRefreshToken);

        var claims = new Claim[]
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("userName", user.Name.ToString()),
            new Claim("userEmail", user.Email.ToString())
        };


        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            claims: claims,
            notBefore: DateTime.UtcNow/*notBefore - начиная с какого времени токен может быть валидным*/,
            expires: DateTime.UtcNow.AddDays(_options.RefreshTokenExpiresDays),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<UserEntity?> GetUserFromRefreshToken(string refresh)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(refresh);
        var userId = token?.Claims?.First(c => c.Type == "userId")?.Value;

        return userId is not null
        ? await _userService.GetUser(new Guid(userId))
        : null;
    }
}