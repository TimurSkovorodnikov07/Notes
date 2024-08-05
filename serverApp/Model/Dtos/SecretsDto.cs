using System.ComponentModel.DataAnnotations;

public class UserSecretsDto
{
    [Required]
    public string PostgresPassword { get; set; }

    [Required]
    public string EmailPassword { get; set; }
}