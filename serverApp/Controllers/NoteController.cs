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
    public NoteController(ILogger<NoteController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> NotesGet(int? from = null, int? to = null, string? sortType = null, string? search = null)
    {
        var userId = User?.Claims?.GetIdValue();
        string _authorId = userId is not null ? userId : null;

        // До я юзал пар маршрута, они обез, мне нужно не обез, потому юзаю теперь query 
        if (NoteService.notes.Count <= 0)
            return NotFound();

        List<NoteEntity> selectedNotes = NoteService.notes.Where(n => n.AuthorId == _authorId).ToList();

        if (selectedNotes == null)
            return NotFound();

        if (search != null)
        {
            selectedNotes = selectedNotes.Where((n) => n.Name.ToLower().Contains(search.ToLower())).ToList();
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
    [HttpGet("{id:int}")]
    public async Task<IActionResult> NoteGet([Required] int id)
    {
        var userId = User?.Claims?.GetIdValue();
        string _authorId = userId is not null ? userId : null;

        var note = NoteService.notes.FirstOrDefault((n) => n.Id == id);

        if (note != null)
        {
            if (note.AuthorId != _authorId)
                return Forbid();

            return Ok(note);
        }
        return NotFound();
    }

    [ValidationFilter]

    [HttpPost]
    public IActionResult NoteCreate([FromForm, Required] NoteDto dto)
    {
        var userId = User?.Claims?.GetIdValue();
        string _authorId = userId is not null ? userId : null;

        var newNote = NoteEntity.Create(dto, _authorId);

        if (newNote is not null)
        {
            NoteService.notes.Add(newNote);
            return Ok();
        }
        return BadRequest();
    }
    [ValidationFilter]
    [HttpPut("{id:int}")]
    public IActionResult NoteUpdate([FromForm, Required] NoteChangeDto newValue)
    {
        var userId = User?.Claims?.GetIdValue();
        string _authorId = userId is not null ? userId : null;

        var changeableNote = NoteService.notes.FirstOrDefault(x => x.Id == newValue.Id);

        if (changeableNote != null)
        {
            if (changeableNote.AuthorId != _authorId)
                return Forbid();

            changeableNote.Name = newValue.NewName;
            changeableNote.Discription = newValue.NewDiscription;

            return Ok();
        }

        return NotFound();
    }
    [ValidationFilter]
    [HttpDelete("{id:int}")]
    public IActionResult NoteDelete([Required] int id)
    {
        var userId = User?.Claims?.GetIdValue();
        string _authorId = userId is not null ? userId : null;

        var deleteNote = NoteService.notes.FirstOrDefault((n) => n.Id == id);

        if (deleteNote != null)
        {
            if (deleteNote.AuthorId != _authorId)
                return Forbid();

            NoteService.notes.Remove(deleteNote);
            return Ok();
        }

        return NotFound();
    }
}