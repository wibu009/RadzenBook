﻿using System.Reflection;
using RadzenBook.Application.Common.Persistence;
using RadzenBook.Domain.Common.Contracts;
using RadzenBook.Infrastructure.Persistence.Repositories;

namespace RadzenBook.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly DbContext _context;
    private readonly Dictionary<Type, object> _repositories;
    private bool _disposed;

    public UnitOfWork(DbContext dbContext)
    {
        _context = dbContext;
        _repositories = new Dictionary<Type, object>();
    }

    public TIRepository GetRepository<TIRepository, TEntity, TKey>()
        where TIRepository : IBaseRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        var type = typeof(TIRepository);

        //check if the repository is already in the dictionary
        if (_repositories.TryGetValue(type, out var repository))
        {
            return (TIRepository)repository;
        }

        //get class implementing BaseRepository<TEntity, TKey>
        var baseRepositoryType = typeof(BaseRepository<TEntity, TKey>);

        //get class implementing TRepository with DbContext as constructor parameter
        var repositoryType = Assembly.GetAssembly(baseRepositoryType)?.GetTypes().FirstOrDefault(t =>
            t.BaseType == baseRepositoryType && t.GetInterfaces().Any(i => i == type) && t.GetConstructors()
                .Any(c => c.GetParameters().Any(p => p.ParameterType == typeof(DbContext))));
        var repositoryInstance =
            Activator.CreateInstance(repositoryType ?? throw new InvalidOperationException("Repository not found"),
                _context);
        _repositories.TryAdd(type, repositoryInstance ?? throw new InvalidOperationException("Repository not found"));
        return (TIRepository)repositoryInstance;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.CommitTransactionAsync(cancellationToken);

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.RollbackTransactionAsync(cancellationToken);

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        await _context.DisposeAsync();
        _disposed = true;
    }
}