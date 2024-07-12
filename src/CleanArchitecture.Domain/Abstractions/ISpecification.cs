using System.Linq.Expressions;

namespace CleanArchitecture.Domain.Abstractions;

/// <summary>
/// Interfaz que define las especificaciones de consulta para las entidades.
/// </summary>
public interface ISpecification<TEntity, EntityId>
    where TEntity : Entity<EntityId>
    where EntityId : class
{
    Expression<Func<TEntity, bool>>? Criteria { get; }
    List<Expression<Func<TEntity, object>>> Includes { get; }
    Expression<Func<TEntity, object>>? OrderBy { get; }
    Expression<Func<TEntity, object>>? OrderByDescending { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}
