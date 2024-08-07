public class QueryCreaterService
{
    private readonly ILogger<QueryCreaterService> _logger;
    public QueryCreaterService(ILogger<QueryCreaterService> logger)
    {
        _logger = logger;
    }

    public string AddQuery(string tableName, params string[] valuesParamTypes)
    {
        string sqlQuery = @$"INSERT INTO {tableName} VALUES ({Union(valuesParamTypes)});";
        _logger.LogDebug(sqlQuery);


        return sqlQuery;
    }

    public string UpdateQuery(string tableName, params string[] valuesParamTypes)
    {
        string sqlQuery = @$"UPDATE {tableName} SET ({Union(valuesParamTypes)});";
        _logger.LogDebug(sqlQuery);

        return sqlQuery;
    }

    private string Union(params string[] parameters)
    {
        string result = "";

        for (var i = 0; i < parameters.Length; i++)
        {
            result += $"@{parameters[i]}";

            if (i < parameters.Length - 1)
                result += ",";
        }
        return result;
    }
}