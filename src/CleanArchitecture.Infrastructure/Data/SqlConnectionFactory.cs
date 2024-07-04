using System.Data;
using CleanArchitecture.Application.Abstractions.Data;
using Microsoft.Data.SqlClient;

namespace CleanArchitecture.Infrastructure.Data;

/// <summary>
/// Factoría de conexión que se utilizará para Dapper (consultas).
/// </summary>
internal sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}
