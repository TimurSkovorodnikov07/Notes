using System.ComponentModel.DataAnnotations;

public class NoteChangeDto
{
    [Required]
    public Guid Id { get; set; }

    [Required, StringLength(120)]
    public string NewName { get; set; }

    [StringLength(5000)]
    public string? NewDescription { get; set; }

}