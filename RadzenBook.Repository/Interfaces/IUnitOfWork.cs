using FirstBlazorProject_BookStore.Entity;
using FirstBlazorProject_BookStore.Repository.Implements;

namespace FirstBlazorProject_BookStore.Repository.Interfaces;

public interface IUnitOfWork
{
    public TRepository GetRepository<TRepository, TEntity, TKey>()
        where TRepository : BaseRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task DisposeAsync();
}