using System.ComponentModel.DataAnnotations;

public class UserDto
{
    [Required]
    public string Id { get; set; }

    [Required, StringLength(24)]
    public string Name { get; set; }

    [Required, StringLength(45)]
    public string Email { get; set; }
}