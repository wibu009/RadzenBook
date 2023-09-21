using System.Linq.Expressions;

namespace RadzenBook.Application.Common.Persistence.Repositories;

public interface IBaseRepository<TEntity, in TKey> where TEntity : BaseEntity<TKey>
{
    Task<List<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null,
        bool isTracking = false,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null,
        bool isTracking = false,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(TKey id, string? includeProperties = null, bool isTracking = false,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null,
        CancellationToken cancellationToken = default);

    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteByPropsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);

    Task SoftDeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);

    Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task SoftDeleteByPropsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

    Task SoftDeleteRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
}