using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/notes")]
[Authorize]
public class NoteController : ControllerBase
{
    //я Даун тк фильтр Authorize работает ПОСЛЕ создания контроллера и работы конструктора.
    //Я вобщем проебал 3-4 часа не понимая почему айди юзера в контоллере = null
    //Думал у меня проверка валидности в Program не работает коректно, нет, вроде работает когда я с токеном кидал запрос например на /login получал 401, как и пологалось
    //КОроче решением было то что я юзер айди брал уже в actions а не конструкторе, 
    //https://qna.habr.com/q/1363664
    private readonly ILogger<NoteController> _logger;
    private readonly NoteService _noteService;
    public NoteController(ILogger<NoteController> logger, NoteService noteService)
    {
        _logger = logger;
        _noteService = noteService;
    }

    [HttpGet]
    public async Task<IActionResult> NotesGet(int? from = null, int? to = null, string? sortType = null, string? search = null)
    {
        var userId = User?.Claims?.GetIdValue();
        string authorId = userId is not null ? userId : null;

        // До я юзал пар маршрута, они обез, мне нужно не обез, потому юзаю теперь query 

        List<NoteEntity> selectedNotes =
            (await _noteService.GetNotesByAuthorId(authorId)).ToList();

        if (selectedNotes == null)
            return NotFound();

        if (search != null)
        {
            selectedNotes = selectedNotes
                .Where((n) => n.Name.ToLower().Contains(search.ToLower()))
                .ToList();
        }

        selectedNotes =
         sortType == "date"
         ? selectedNotes.OrderBy((n) => n.TimeOfCreation).ToList()
         : selectedNotes.OrderByDescending((n) => n.TimeOfCreation).ToList();


        if (from != null && to != null)
        {
            var taken = selectedNotes.GetValues((int)from, (int)to);

            if (taken != null)
                selectedNotes = taken;
        }
        HttpContext.Response.Headers.Add("selectedNotesTotalCount",
            selectedNotes.Count.ToString());//Клиенту для пагинации нужно снать колл нотов

        return Ok(selectedNotes);
    }
    [ValidationFilter]
    [HttpGet("{id}")]
    public async Task<IActionResult> NoteGet([Required] Guid id)
    {
        var userId = User?.Claims?.GetIdValue();
        string authorId = userId is not null ? userId : null;

        var note = await _noteService.GetNote(id);

        if (note != null)
        {
            if (note.AuthorId.Equals(authorId))
                return Forbid();

            return Ok(note);
        }
        return NotFound();
    }

    [ValidationFilter]
    [HttpPost]
    public async Task<IActionResult> NoteCreate([FromForm] NoteDto dto)
    {
        var userId = User?.Claims?.GetIdValue();
        string authorId = userId is not null ? userId : null;

        var newNote = NoteEntity.Create(dto, authorId);

        if (newNote is not null)
        {
            await _noteService.Add(newNote);
            return Ok();
        }
        return BadRequest();
    }

    [ValidationFilter]
    [HttpPut]
    public async Task<IActionResult> NoteUpdate([FromForm] NoteChangeDto newValue)
    {
        var userId = User?.Claims?.GetIdValue();
        string authorId = userId is not null ? userId : null;

        var changableNote = await _noteService.GetNote(newValue.Id);

        if (changableNote is not null)
        {
            if (changableNote.AuthorId.Equals(authorId))
                return Forbid();

            await _noteService.NameAndDescriptionUpdate(newValue);

            return Ok();
        }

        return NotFound();
    }
    [ValidationFilter]
    [HttpDelete("{id}")]
    public async Task<IActionResult> NoteDelete([Required] Guid id)
    {
        var userId = User?.Claims?.GetIdValue();
        string authorId = userId is not null ? userId : null;

        var deleteNote = await _noteService.GetNote(id);

        if (deleteNote != null)
        {
            if (deleteNote.AuthorId.Equals(authorId))
                return Forbid();

            await _noteService.Remove(deleteNote.Id);
            return Ok();
        }

        return NotFound();
    }
}