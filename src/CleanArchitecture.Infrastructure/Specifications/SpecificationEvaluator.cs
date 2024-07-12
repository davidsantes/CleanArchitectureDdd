using CleanArchitecture.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Specifications;

/// <summary>
/// Clase que evalúa las especificaciones de consulta antes de enviarlas a la base de datos.
/// Utiliza Entity Framework Core para aplicar los criterios, inclusiones, ordenamientos y paginación.
/// </summary>
public class SpecificationEvaluator<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : class
{
    /// <summary>
    /// Método que evalúa una especificación de consulta y la aplica a la misma.
    /// </summary>
    /// <param name="inputQuery">La consulta inicial a la que se aplicarán las especificaciones.</param>
    /// <param name="specification">La especificación que define los criterios, inclusiones, ordenamientos y paginación.</param>
    /// <returns>Un <see cref="IQueryable{TEntity}"/> que representa la consulta modificada según la especificación.</returns>
    public static IQueryable<TEntity> GetQuery(
        IQueryable<TEntity> inputQuery,
        ISpecification<TEntity, TEntityId> specification
    )
    {
        if (specification.Criteria is not null)
        {
            inputQuery = inputQuery.Where(specification.Criteria);
        }

        if (specification.OrderBy is not null)
        {
            inputQuery = inputQuery.OrderBy(specification.OrderBy);
        }
        else if (specification.OrderByDescending is not null)
        {
            inputQuery = inputQuery.OrderByDescending(specification.OrderByDescending);
        }

        if (specification.IsPagingEnabled)
        {
            inputQuery = inputQuery.Skip(specification.Skip).Take(specification.Take);
        }

        inputQuery = specification
            .Includes!.Aggregate(inputQuery, (current, include) => current.Include(include))
            .AsSplitQuery()
            .AsNoTracking();

        return inputQuery;
    }
}
