using System.Linq.Expressions;
using FirstBlazorProject_BookStore.Entity;
using FirstBlazorProject_BookStore.Model.Core;
using FirstBlazorProject_BookStore.Model.Cores;

namespace FirstBlazorProject_BookStore.Repository.Interfaces;

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

    public Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    public Task DeleteByPropsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    public Task SoftDeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);

    public Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    public Task SoftDeleteByPropsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

    public Task SoftDeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    public Task<PaginatedList<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null,
        bool isTracking = true,
        CancellationToken cancellationToken = default);
}