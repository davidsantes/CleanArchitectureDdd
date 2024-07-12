using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

internal abstract class Repository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : class
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<TEntity?> GetByIdAsync(
        TEntityId id,
        CancellationToken cancellationToken = default
    )
    {
        return await DbContext
            .Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(TEntity entity)
    {
        DbContext.Add(entity);
    }

    /// <summary>
    /// Método que aplica una especificación de consulta a una consulta de entidades.
    /// </summary>
    public IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TEntityId> specification)
    {
        return SpecificationEvaluator<TEntity, TEntityId>.GetQuery(
            DbContext.Set<TEntity>().AsQueryable(),
            specification
        );
    }

    /// <summary>
    /// Método que obtiene todas las entidades que cumplen con una especificación de consulta.
    /// </summary>
    public async Task<IReadOnlyList<TEntity>> GetAllWithSpec(
        ISpecification<TEntity, TEntityId> specification
    )
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    /// <summary>
    /// Método que obtiene la cantidad de entidades que cumplen con una especificación de consulta.
    /// </summary>
    public async Task<int> CountAsync(ISpecification<TEntity, TEntityId> specification)
    {
        return await ApplySpecification(specification).CountAsync();
    }
}
