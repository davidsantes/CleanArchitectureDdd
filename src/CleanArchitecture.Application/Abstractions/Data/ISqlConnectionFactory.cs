using System.Data;

namespace CleanArchitecture.Application.Abstractions.Data;

/// <summary>
/// Factor�a de conexi�n que se utilizar� para Dapper (consultas).
/// </summary>
public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}