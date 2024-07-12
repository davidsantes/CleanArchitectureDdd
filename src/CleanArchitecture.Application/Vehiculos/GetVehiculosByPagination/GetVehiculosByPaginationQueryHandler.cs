using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Domain.Vehiculos.Specifications;

namespace CleanArchitecture.Application.Vehiculos.GetVehiculosByPagination;

/// <summary>
/// Clase para manejar la consulta de obtener vehículos por paginación.
/// </summary>
internal sealed class GetVehiculosByPaginationQueryHandler
    : IQueryHandler<GetVehiculosByPaginationQuery, PaginationResult<Vehiculo, VehiculoId>>
{
    private readonly IVehiculoRepository _vehiculoRepository;

    public GetVehiculosByPaginationQueryHandler(IVehiculoRepository vehiculoRepository)
    {
        _vehiculoRepository = vehiculoRepository;
    }

    public async Task<Result<PaginationResult<Vehiculo, VehiculoId>>> Handle(
        GetVehiculosByPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var specification = new VehiculoPaginationSpecification(
            request.SortBy!,
            request.PageIndex,
            request.PageSize,
            request.Modelo!
        );

        var vehiculos = await _vehiculoRepository.GetAllWithSpec(specification);
        var specificationCount = new VehiculoPaginationCountingSpecification(request.Modelo!);
        var totalRecords = await _vehiculoRepository.CountAsync(specificationCount);

        var rounded = Math.Ceiling(
            Convert.ToDecimal(totalRecords) / Convert.ToDecimal(request.PageSize)
        );
        var totalPages = Convert.ToInt32(rounded);
        var vehiculosByPage = vehiculos.Count();

        return new PaginationResult<Vehiculo, VehiculoId>
        {
            Count = totalRecords,
            Data = vehiculos.ToList(),
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            ResultByPage = vehiculosByPage
        };
    }
}
