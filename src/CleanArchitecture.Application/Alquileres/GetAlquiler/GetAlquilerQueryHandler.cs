using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using Dapper;

namespace CleanArchitecture.Application.Alquileres.GetAlquiler;

/// <summary>
/// Se utiliza para manejar consultas de obtención de alquileres.
/// Esta clase es internal porque su funcionalidad no debe ser accesible desde fuera del ensamblado en el que se encuentra.
/// La clase que sí es pública a la capa de presentación es <see cref="GetAlquilerQuery"/>.
/// </summary>
internal sealed class GetAlquilerQueryHandler : IQueryHandler<GetAlquilerQuery, AlquilerResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAlquilerQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<AlquilerResponse>> Handle(
        GetAlquilerQuery request,
        CancellationToken cancellationToken
    )
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = """
               SELECT
                    id AS Id,
                    VehiculoId,
                    UserId,
                    status AS Status,
                    PrecioPorPeriodo_Monto AS PrecioAlquiler,
                    PrecioPorPeriodo_TipoMoneda AS TipoMonedaAlquiler,
                    Mantenimiento_Monto AS PrecioMantenimiento,
                    Mantenimiento_TipoMoneda AS TipoMonedaMantenimiento,
                    Accesorios_Monto AS AccesoriosPrecio,
                    Accesorios_TipoMoneda AS TipoMonedaAccesorio,
                    PrecioTotal_Monto AS PrecioTotal,
                    PrecioTotal_TipoMoneda AS PrecioTotalTipoMoneda,
                    Duracion_Inicio AS DuracionInicio,
                    Duracion_Fin AS DuracionFinal,
                    FechaCreacion AS FechaCreacion
               FROM alquileres WHERE id=@AlquilerId
            """;

        var alquiler = await connection.QueryFirstOrDefaultAsync<AlquilerResponse>(
            sql,
            new { request.AlquilerId }
        );

        return alquiler!;
    }
}
