using System.ComponentModel.DataAnnotations;

public class NoteEntity
{
    public NoteEntity()
    {
        Id = Guid.NewGuid();
    }
    [Required]
    public Guid Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; }

    [Required]
    public DateTime TimeOfCreation { get; set; }


    [StringLength(5000)]
    public string? Description { get; set; }

    [Required]
    public Guid AuthorId { get; set; }

    public static NoteEntity? Create(string name, string description, DateTime timeOfCreation, string authorId)
    {
        var note = new NoteEntity
        {
            Name = name,
            Description = description,
            TimeOfCreation = timeOfCreation,
            AuthorId = new Guid(authorId),
        };

        if (NoteStaticValidator.IsValid(note))
            return note;

        return null;
    }
    public static NoteEntity? Create(NoteDto dto, string authorId) =>
        Create(dto.Name, dto.Description, DateTime.UtcNow, authorId);
}