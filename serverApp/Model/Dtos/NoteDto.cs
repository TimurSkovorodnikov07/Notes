#nullable disable
using System.ComponentModel.DataAnnotations;
public class NoteDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Discription { get; set; }
}