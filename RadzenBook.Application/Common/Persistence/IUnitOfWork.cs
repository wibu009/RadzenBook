namespace RadzenBook.Application.Common.Persistence;

public interface IUnitOfWork
{
    TIRepository GetRepository<TIRepository, TEntity, TKey>()
        where TIRepository : IBaseRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>;
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    ValueTask DisposeAsync();
}