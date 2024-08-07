using System.ComponentModel.DataAnnotations;

public class UserRegistrationDto
{
    [Required, StringLength(24)]
    public string Name { get; set; }

    [Required, StringLength(45)]
    public string Email { get; set; }

    [Required,
    RegularExpression(UserLoginDto.PASSWORDREGEX)]
    public string Password { get; set; }
}