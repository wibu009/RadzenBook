using System.Linq.Expressions;
using FirstBlazorProject_BookStore.Entity;
using FirstBlazorProject_BookStore.Model.Core;
using FirstBlazorProject_BookStore.Model.Cores;
using FirstBlazorProject_BookStore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirstBlazorProject_BookStore.Repository.Implements;

public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    protected readonly DbSet<TEntity> DbSet;

    protected BaseRepository(DbContext context)
    {
        DbSet = context.Set<TEntity>();
    }

    #region Query Methods

    public virtual async Task<IEnumerable<TEntity?>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null,
        bool isTracking = true,
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
                foreach (var includeProperty in includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = await Task.FromResult(orderBy(query));
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.Where(e => e.IsDeleted == false).ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, string? includeProperties = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            IQueryable<TEntity> query = DbSet;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync(e => e.IsDeleted == false && e.Id!.Equals(id), cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task<PaginatedList<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null,
        bool isTracking = true,
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
                foreach (var includeProperty in includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = await Task.FromResult(orderBy(query));
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            var count = await query.Where(e => !e.IsDeleted).CountAsync(cancellationToken);
            var items = await query.Where(e => !e.IsDeleted).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<TEntity>(items, count, pageNumber, pageSize);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
            throw new Exception(e.Message);
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
            throw new Exception(e.Message);
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
            throw new Exception(e.Message);
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
            throw new Exception(e.Message);
        }
    }

    public virtual async Task DeleteByPropsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await GetAsync(filter, cancellationToken: cancellationToken);
            await DeleteRangeAsync(entities!, cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            await Task.Run(() => DbSet.RemoveRange(entities), cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            entity.IsDeleted = true;
            await Task.Run(() => DbSet.Update(entity), cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
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
            throw new Exception(e.Message);
        }
    }

    public virtual async Task SoftDeleteByPropsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await GetAsync(filter, cancellationToken: cancellationToken);
            await SoftDeleteRangeAsync(entities!, cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task SoftDeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
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