using System.ComponentModel.DataAnnotations;

public class UserLoginDto
{
    [Required, StringLength(45)]
    public string Email { get; set; }

    [Required,
    RegularExpression(PASSWORDREGEX)]
    public string Password { get; set; }

    public const string PASSWORDREGEX
        = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^\w\s]).{8,24}$";
}