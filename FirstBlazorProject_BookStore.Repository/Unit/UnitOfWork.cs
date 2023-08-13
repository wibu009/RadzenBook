using FirstBlazorProject_BookStore.DataAccess.Entities;
using FirstBlazorProject_BookStore.Repository.Base;

namespace FirstBlazorProject_BookStore.Repository.Unit;

public class UnitOfWork : IUnitOfWork
{
    public IBaseRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }
}