using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Vehiculos.Specifications;

/// <summary>
/// Clase de especificación para contar los vehículos.
/// </summary>
public class VehiculoPaginationCountingSpecification : BaseSpecification<Vehiculo, VehiculoId>
{
    public VehiculoPaginationCountingSpecification(string search)
        : base(x => string.IsNullOrEmpty(search) || x.Modelo == new Modelo(search)) { }
}
