using FirstBlazorProject_BookStore.DataAccess.Context;
using FirstBlazorProject_BookStore.Entity;
using FirstBlazorProject_BookStore.Repository.Base;

namespace FirstBlazorProject_BookStore.Repository.Unit;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly BookStoreDataContext _bookStoreDataContext;
    private readonly Dictionary<Type, object> _repositories;
    private bool _disposed;

    public UnitOfWork(BookStoreDataContext bookStoreDataContext)
    {
        _bookStoreDataContext = bookStoreDataContext;
        _repositories = new Dictionary<Type, object>();
    }

    public TRepository GetRepository<TRepository, TEntity, TKey>()
        where TRepository : BaseRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return (TRepository)(_repositories[typeof(TEntity)]);
        }

        var repository = Activator.CreateInstance(typeof(TRepository), _bookStoreDataContext);
        _repositories.Add(typeof(TEntity), repository!);
        return ((TRepository)repository!)!;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _bookStoreDataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    private async Task DisposeAsync(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                await _bookStoreDataContext.DisposeAsync();
            }
        }

        _disposed = true;
    }
}