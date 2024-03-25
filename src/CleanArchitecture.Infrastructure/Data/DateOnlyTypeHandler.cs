using Dapper;
using System.Data;

namespace CleanArchitecture.Infrastructure.Data;

/// <summary>
/// Clase para manejar la conversi�n entre el tipo de datos .NET DateOnly y el tipo de datos de la base de datos.
/// </summary>
internal sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    /// <summary>
    /// Convierte un objeto de la base de datos al tipo de datos .NET DateOnly.
    /// </summary>
    /// <param name="value">Valor de la base de datos.</param>
    /// <returns>Objeto DateOnly convertido.</returns>
    public override DateOnly Parse(object value) => DateOnly.FromDateTime((DateTime)value);

    /// <summary>
    /// Establece el valor del par�metro de base de datos con el valor DateOnly.
    /// </summary>
    /// <param name="parameter">Par�metro de base de datos.</param>
    /// <param name="value">Valor DateOnly.</param>
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value;
    }
}