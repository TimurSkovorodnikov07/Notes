using System.Security.Claims;

public static class ClaimsExtension
{
    private static string GetValue(Claim[] claims, string selectType) => claims.FirstOrDefault(c => c.Type == selectType).Value;
    private static string GetValue(IEnumerable<Claim> claims, string selectType) => claims.FirstOrDefault(c => c.Type == selectType).Value;

    public static string GetIdValue(this Claim[] claims) => GetValue(claims, "userId");
    public static string GetNameValue(this Claim[] claims) => GetValue(claims, "userName");
    public static string GetEmailValue(this Claim[] claims) => GetValue(claims, "userEmail");

    public static string GetIdValue(this IEnumerable<Claim> claims) => GetValue(claims, "userId");
    public static string GetNameValue(this IEnumerable<Claim> claims) => GetValue(claims, "userName");
    public static string GetEmailValue(this IEnumerable<Claim> claims) => GetValue(claims, "userEmail");
}