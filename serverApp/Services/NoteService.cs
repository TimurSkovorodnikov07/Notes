using Dapper;
public class NoteService
{
    public NoteService(ConnectionFactory factory,
     QueryCreaterService queryCreater, ILogger<NoteService> logger)
    {
        _factory = factory;
        _queryCreater = queryCreater;
        _logger = logger;
    }
    private readonly ConnectionFactory _factory;
    private readonly ILogger<NoteService> _logger;
    private readonly QueryCreaterService _queryCreater;

    public async Task<bool> Add(NoteEntity note)
    {
        using var dbCon = _factory.Create();
        var queryPattern = AddOrUpdateSqlQueryPattern();

        return await dbCon
            .ExecuteAsync(
                _queryCreater.AddQuery(queryPattern.Item1, queryPattern.Item2),
            ParamsForAddOrUpdateQuery(note)) >= 0;
    }




    public async Task<IEnumerable<NoteEntity>> GetNotesByAuthorId(string authorId)
    {
        return await GetNotes("author_id", authorId);
    }
    public async Task<NoteEntity?> GetNote(Guid guid)
    {
        return (await GetNotes("id", guid.ToString())).FirstOrDefault();
    }
    public async Task<IEnumerable<NoteEntity>> GetNotes(string filterBy, string value)
    {
        using var dbCon = _factory.Create();

        string sqlQuery = @$"SELECT  
            id as {nameof(NoteEntity.Id)}, 
            name as {nameof(NoteEntity.Name)}, 
            description as {nameof(NoteEntity.Description)}, 
            time_of_creation as {nameof(NoteEntity.TimeOfCreation)}, 
            author_id as {nameof(NoteEntity.AuthorId)} 
            FROM notes 
            WHERE {filterBy} = @valueParam ;";

        _logger.LogDebug(sqlQuery);

        return await dbCon.QueryAsync<NoteEntity>(sqlQuery,
        new
        {
            valueParam = value
        });
    }




    public async Task<bool> NameAndDescriptionUpdate(NoteChangeDto newValue)
    {
        using var dbCon = _factory.Create();

        var sqlQuery = "UPDATE notes SET name = @newNameParam, Description = @newDesParam" +
            " WHERE id = @guidStringParam";

        _logger.LogDebug(sqlQuery);

        return await dbCon.ExecuteAsync(sqlQuery, new
        {
            newNameParam = newValue.NewName,
            newDesParam = newValue.NewDescription,
            guidStringParam = newValue.Id.ToString()
        }) >= 0;
    }
    public async Task<bool> Update(NoteEntity note)
    {
        using var dbCon = _factory.Create();
        var queryPattern = AddOrUpdateSqlQueryPattern();

        var sqlQuery = _queryCreater.UpdateQuery(queryPattern.Item1, queryPattern.Item2);
        _logger.LogDebug(sqlQuery);

        return await dbCon.ExecuteAsync(sqlQuery, ParamsForAddOrUpdateQuery(note)) >= 0;
    }
    public async Task<bool> Remove(Guid guid)
    {
        using var dbCon = _factory.Create();
        string sqlQuery = @$"DELETE FROM notes WHERE id = @filterId ;";

        _logger.LogDebug(sqlQuery);

        return await dbCon.ExecuteAsync(sqlQuery, new { filterId = guid.ToString() }) >= 0;
    }



    private (string, string[]) AddOrUpdateSqlQueryPattern()
    {
        return ("notes", new string[] { "id", "name", "des", "toc", "authId" });
    }
    private object ParamsForAddOrUpdateQuery(NoteEntity note)
    {
        return new
        {
            id = note.Id.ToString(),
            name = note.Name,
            des = note.Description,
            toc = note.TimeOfCreation,
            authId = note.AuthorId
        };
    }
}