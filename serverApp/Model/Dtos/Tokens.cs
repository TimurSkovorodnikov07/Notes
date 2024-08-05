using System.ComponentModel.DataAnnotations;

public class Tokens
{
    public Tokens(string access, string refresh)
    {
        AccessToken = access;
        RefreshToken = refresh;
    }

    [Required]
    public string AccessToken { get; set; }
    [Required]
    public string RefreshToken { get; set; }
}