public class RefreshTokenService
{
    public static List<RefreshToken> RefreshTokens = new List<RefreshToken>();
    public static async Task AddRefreshToken(Guid userId, string token)
    {
        RefreshTokens.Add(new RefreshToken { UserId = userId, Token = token });
    }
    public static async Task SetRefreshToken(Guid userId, string newToken)
    {
        RefreshTokens.First(x => x.UserId == userId).Token = newToken;
    }
    public static bool RefreshTokenVerify(string refresh)
    {
        var findToken = RefreshTokens.FirstOrDefault(t => t.Token == refresh);

        if (findToken is not null)
            return true;

        return false;
    }
}