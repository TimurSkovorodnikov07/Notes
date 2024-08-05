using System.ComponentModel.DataAnnotations;

public class RefreshToken
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string Token { get; set; }
}