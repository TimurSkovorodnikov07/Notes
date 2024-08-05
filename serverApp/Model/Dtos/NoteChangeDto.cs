using System.ComponentModel.DataAnnotations;

public class NoteChangeDto
{
    [Required] public int Id { get; set; }
    [Required] public string NewName { get; set; }
    [Required] public string NewDiscription { get; set; }

}