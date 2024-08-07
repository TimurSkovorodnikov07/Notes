using System.ComponentModel.DataAnnotations;
public class UserEntity
{
    public UserEntity()
    {
        Id = Guid.NewGuid();
    }

    [Required]
    public Guid Id { get; set; }

    [Required, StringLength(24)]
    public string Name { get; set; }

    [Required, StringLength(45)]
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