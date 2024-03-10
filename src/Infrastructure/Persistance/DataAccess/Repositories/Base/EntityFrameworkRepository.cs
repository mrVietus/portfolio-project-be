using System.Linq.Expressions;
using Crawler.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Infrastructure.Persistance.DataAccess.Repositories.Base;

public class EntityFrameworkRepository<TEntity>(DbContext context) 
    : IRepository<TEntity> where TEntity : class
{
    protected DbContext Context { get; } = context;
    protected DbSet<TEntity> DbSet { get; } = context.Set<TEntity>();

    private static readonly char[] separator = [','];

    public async Task<IList<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        bool asNoTracking = false)
    {
        IQueryable<TEntity> query = GetQuery(filter, orderBy, includeProperties);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(object id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "", bool asNoTracking = false)
    {
        IQueryable<TEntity> query = GetQuery(filter, null, includeProperties);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.AnyAsync();
    }

    public async Task InsertAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public void Insert(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public void InsertRange(IEnumerable<TEntity> entities)
    {
        DbSet.AddRange(entities);
    }

    public void Update(TEntity entityToUpdate)
    {
        DbSet.Attach(entityToUpdate);
        Context.Entry(entityToUpdate).State = EntityState.Modified;
    }

    public void UpdateRange(IEnumerable<TEntity> entitiesToUpdate)
    {
        DbSet.UpdateRange(entitiesToUpdate);
    }

    public void Delete(TEntity entityToDelete)
    {
        if (Context.Entry(entityToDelete).State == EntityState.Detached)
        {
            DbSet.Attach(entityToDelete);
        }

        DbSet.Remove(entityToDelete);
    }

    public void DeleteRange(IEnumerable<TEntity> entitiesToDelete)
    {
        DbSet.RemoveRange(entitiesToDelete);
    }

    private IQueryable<TEntity> GetQuery(
        Expression<Func<TEntity, bool>>? filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
        string includeProperties)
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includeProperties.Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return query;
    }
}
