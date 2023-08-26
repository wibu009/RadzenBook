using FirstBlazorProject_BookStore.Entity;
using FirstBlazorProject_BookStore.Repository.Base;

namespace FirstBlazorProject_BookStore.Repository.Unit;

public interface IUnitOfWork
{
    public TRepository GetRepository<TRepository, TEntity, TKey>()
        where TRepository : BaseRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task DisposeAsync();
}