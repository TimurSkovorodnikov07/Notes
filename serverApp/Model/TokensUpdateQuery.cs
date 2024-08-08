using System.ComponentModel.DataAnnotations;

public class TokensUpdateQuery
{
    [Required] public string RefreshToken { get; set; }
}