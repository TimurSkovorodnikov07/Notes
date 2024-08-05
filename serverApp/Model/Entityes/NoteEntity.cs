using System.ComponentModel.DataAnnotations;

public class NoteEntity
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public DateTime TimeOfCreation { get; set; }

    public string Discription { get; set; }

    [Required]
    public string AuthorId { get; set; }

    public static NoteEntity? Create(string name, string discription, DateTime timeOfCreation, string authorId)
    {
        var curId = NoteService.notes.LastOrDefault()?.Id ?? 0;

        var note = new NoteEntity
        {
            Id = curId + 1,
            Name = name,
            Discription = discription,
            TimeOfCreation = timeOfCreation,
            AuthorId = authorId,
        };

        if (NoteStaticValidator.IsValid(note))
            return note;

        return null;
    }
    public static NoteEntity? Create(NoteDto dto, string authorId) =>
        Create(dto.Name, dto.Discription, DateTime.UtcNow, authorId);
}