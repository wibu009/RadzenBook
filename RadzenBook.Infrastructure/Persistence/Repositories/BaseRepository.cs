using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RadzenBook.Application.Common.Persistence.Repositories;
using RadzenBook.Domain.Entities;
using RadzenBook.Domain.Exceptions;

namespace RadzenBook.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    #region Properties
    protected readonly DbSet<TEntity> DbSet;
    #endregion

    #region Constructors
    protected BaseRepository(DbContext context)
    {
        DbSet = context.Set<TEntity>();
    }
    #endregion

    #region Query Methods
    public virtual async Task<List<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null,
        bool isTracking = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = await Task.FromResult(query.Where(filter));
            }

            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            if (orderBy != null)
            {
                query = await Task.FromResult(orderBy(query));
            }
            else
            {
                query = query.OrderBy(e => e.CreatedAt);
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.Where(e => e.IsDeleted == false).ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw RepositoryException.Create(MethodBase.GetCurrentMethod()?.Name!, GetType().Name, e.Message, e);
        }
    }

    public virtual async Task<TEntity?> GetByIdAsync(
        TKey id, 
        string? includeProperties = null, 
        bool isTracking = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            IQueryable<TEntity> query = DbSet;
            
            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            
            return await query.Where(e => e.IsDeleted == false).FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
        }
        catch (Exception e)
        {
            throw RepositoryException.Create(MethodBase.GetCurrentMethod()?.Name!, GetType().Name, e.Message, e);
        }
    }

    public virtual async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = await Task.FromResult(query.Where(filter));
            }

            return await query.Where(e => e.IsDeleted == false).CountAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw RepositoryException.Create(MethodBase.GetCurrentMethod()?.Name!, GetType().Name, e.Message, e);
        }
    }

    public virtual async Task<List<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null,
        bool isTracking = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = await Task.FromResult(query.Where(filter));
            }

            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            if (orderBy != null)
            {
                query = await Task.FromResult(orderBy(query));
            }
            else
            {
                query = query.OrderBy(e => e.CreatedAt);
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.Where(e => e.IsDeleted == false)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw RepositoryException.Create(MethodBase.GetCurrentMethod()?.Name!, GetType().Name, e.Message, e);
        }
    }

    #endregion

    #region Command Methods

    public virtual async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }
        catch (Exception e)
        {
            throw RepositoryException.Create(MethodBase.GetCurrentMethod()?.Name!, GetType().Name, e.Message, e);
        }
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            entity.ModifiedAt = DateTime.UtcNow;
            await Task.Run(() => DbSet.Update(entity), cancellationToken);
        }
        catch (Exception e)
        {
            throw RepositoryException.Create(MethodBase.GetCurrentMethod()?.Name!, GetType().Name, e.Message, e);
        }
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await Task.Run(() => DbSet.Remove(entity), cancellationToken);
        }
        catch (Exception e)
        {
            throw RepositoryException.Create(MethodBase.GetCurrentMethod()?.Name!, GetType().Name, e.Message, e);
        }
    }

    public virtual async Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);
            await DeleteAsync(entity!, cancellationToken);
        }
        catch (Exception e)
        {
            throw RepositoryException.Create(MethodBase.GetCurrentMethod()?.Name!, GetType().Name, e.Message, e);
        }
    }

    public virtual async Task DeleteByPropsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await GetAsync(filter, cancellationToken: cancellationToken);
            await DeleteRangeAsync(entities, cancellationToken);
        }
        catch (Exception e)
        {
            throw RepositoryException.Create(MethodBase.GetCurrentMethod()?.Name!, GetType().Name, e.Message, e);
        }
    }

    public virtual async Task DeleteRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            await Task.Run(() => DbSet.RemoveRange(entities), cancellationToken);
        }
        catch (Exception e)
        {
            throw RepositoryException.Create(MethodBase.GetCurrentMethod()?.Name!, GetType().Name, e.Message, e);
        }
    }

    public virtual async Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;
            await Task.Run(() => DbSet.Update(entity), cancellationToken);
        }
        catch (Exception e)
        {
            throw RepositoryException.Create(MethodBase.GetCurrentMethod()?.Name!, GetType().Name, e.Message, e);
        }
    }

    public virtual async Task SoftDeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);
            await SoftDeleteAsync(entity!, cancellationToken);
        }
        catch (Exception e)
        {
            throw RepositoryException.Create(MethodBase.GetCurrentMethod()?.Name!, GetType().Name, e.Message, e);
        }
    }

    public virtual async Task SoftDeleteByPropsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await GetAsync(filter, cancellationToken: cancellationToken);
            await SoftDeleteRangeAsync(entities, cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task SoftDeleteRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.ModifiedAt = DateTime.UtcNow;
            }
            await Task.Run(() => DbSet.UpdateRange(entities), cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    #endregion
}