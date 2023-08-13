using System.Linq.Expressions;
using FirstBlazorProject_BookStore.Common.Core;
using FirstBlazorProject_BookStore.DataAccess.Entities;

namespace FirstBlazorProject_BookStore.Repository.Base;

public interface IBaseRepository<TEntity, in TKey> where TEntity : BaseEntity<TKey>
{
    public Task<IEnumerable<TEntity?>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null,
        bool isTracking = true,
        CancellationToken cancellationToken = default);

    public Task<TEntity?> GetByIdAsync(TKey id, string? includeProperties = null,
        CancellationToken cancellationToken = default);

    public Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    public Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    public Task DeleteAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    public Task SoftDeleteAsync(TKey id, CancellationToken cancellationToken = default);

    public Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    public Task SoftDeleteAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

    public Task SoftDeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    public Task<PaginatedResult<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null,
        bool isTracking = true,
        CancellationToken cancellationToken = default);
}