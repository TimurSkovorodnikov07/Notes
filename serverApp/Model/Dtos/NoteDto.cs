#nullable disable
using System.ComponentModel.DataAnnotations;
public class NoteDto
{
    [Required, StringLength(120)]
    public string Name { get; set; }

    [StringLength(5000)]
    public string? Description { get; set; }
}