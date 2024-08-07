using System.Data;
using Npgsql;

public class ConnectionFactory
{
    public ConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
        NpgsqlGlobalMappers.MappersEnable();
    }
    private readonly string _connectionString;
    public IDbConnection Create()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}