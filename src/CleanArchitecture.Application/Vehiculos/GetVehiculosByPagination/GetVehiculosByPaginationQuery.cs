using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Application.Vehiculos.GetVehiculosByPagination;

/// <summary>
/// Clase que representa la consulta para obtener vehículos paginados.
/// </summary>
public sealed record GetVehiculosByPaginationQuery
    : SpecificationEntry,
        IQuery<PaginationResult<Vehiculo, VehiculoId>>
{
    public string? Modelo { get; init; }
}
