using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Vehiculos.Specifications;

/// <summary>
/// Clase de especificación para paginar los vehículos en función de unos criterios especificados.
/// Hereda de la clase <see cref="BaseSpecification{Vehiculo, VehiculoId}"/>.
/// </summary>
public class VehiculoPaginationSpecification : BaseSpecification<Vehiculo, VehiculoId>
{
    public VehiculoPaginationSpecification(string sort, int pageIndex, int pageSize, string search)
        : base(x => string.IsNullOrEmpty(search) || x.Modelo == new Modelo(search))
    {
        ApplyPaging(pageSize * (pageIndex - 1), pageSize);

        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort)
            {
                case "modeloAsc":
                    AddOrderBy(p => p.Modelo!);
                    break;
                case "modeloDesc":
                    AddOrderByDescending(p => p.Modelo!);
                    break;
                default:
                    AddOrderBy(p => p.FechaUltimaAlquiler!);
                    break;
            }
        }
        else
        {
            AddOrderBy(p => p.FechaUltimaAlquiler!);
        }
    }
}
