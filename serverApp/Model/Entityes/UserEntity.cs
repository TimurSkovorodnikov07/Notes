using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

public class UserEntity
{
    public UserEntity()
    {
        Id = Guid.NewGuid();
    }

    [Required]
    public Guid Id { get; init; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }
    public bool EmailVerify { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    public static UserEntity? Create(string name, string email, string passwordHash)
    {
        var user = new UserEntity
        {
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            EmailVerify = false
        };

        if (UserStaticValidator.IsValid(user))
            return user;

        return null;
    }
    public static UserEntity? Create(UserRegistrationDto dto, string passwordHash) => Create(dto.Name, dto.Email, passwordHash);
}