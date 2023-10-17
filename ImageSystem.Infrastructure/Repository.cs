using ImageSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ImageSystem.Infrastructure;

public class Repository<TEntity> 
    : IRepository<TEntity> where TEntity : Identity
{
    private readonly DbSet<TEntity> _dbSet;
    
    public Repository(PostgreContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public async Task InsertAsync(TEntity entity)
        => await _dbSet.AddAsync(entity);

    public void Delete(TEntity entity)
        => _dbSet.Remove(entity);

    public void Update(TEntity entity)
        => _dbSet.Entry(entity).State = EntityState.Modified;

    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task<TEntity?> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public IQueryable<TEntity> GetQueryable()
        => _dbSet;

    public async Task<IEnumerable<TEntity>> FindAsync
        (Expression<Func<TEntity, bool>> predicate, 
        params Expression<Func<TEntity, object>>[] includes)
    {
        var predicateResult = _dbSet.Where(predicate);
        if (!includes.Any())
            return await predicateResult.ToListAsync();

        foreach (var include in includes)
        {
            predicateResult = predicateResult.Include(include);
        }

        return await predicateResult.ToListAsync();
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool ignoreAutoIncludes = false)
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (ignoreAutoIncludes)
        {
            query = query.IgnoreAutoIncludes();
        }

        return orderBy is not null
            ? await orderBy(query).FirstOrDefaultAsync()
            : await query.FirstOrDefaultAsync();
    }

    public async Task<int> CountAsync()
        => await _dbSet.CountAsync();

    public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        => await _dbSet.AddRangeAsync(entities);
}
