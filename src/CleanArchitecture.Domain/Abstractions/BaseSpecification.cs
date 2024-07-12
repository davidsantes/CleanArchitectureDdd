using System.Linq.Expressions;

namespace CleanArchitecture.Domain.Abstractions;

/// <summary>
/// Clase base para las especificaciones de consulta mediante el patrón specification.
/// </summary>
public abstract class BaseSpecification<TEntity, TEntityId> : ISpecification<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : class
{
    public BaseSpecification() { }

    public BaseSpecification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<TEntity, bool>>? Criteria { get; }
    public List<Expression<Func<TEntity, object>>> Includes { get; } = [];
    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(
        Expression<Func<TEntity, object>> orderByDescendingExpression
    )
    {
        OrderByDescending = orderByDescendingExpression;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}
