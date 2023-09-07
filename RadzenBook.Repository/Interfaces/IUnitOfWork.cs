using FirstBlazorProject_BookStore.Entity;
using FirstBlazorProject_BookStore.Repository.Implements;

namespace FirstBlazorProject_BookStore.Repository.Interfaces;

public interface IUnitOfWork
{
    public TIRepository GetRepository<TIRepository, TEntity, TKey>()
        where TIRepository : IBaseRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>;
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task DisposeAsync();
}