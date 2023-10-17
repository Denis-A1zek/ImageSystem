using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ImageSystem.Domain;

public interface IRepository<TEntity> where TEntity : Identity
{
    Task<TEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync
        (Expression<Func<TEntity, bool>> predicate, 
        params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool ignoreAutoIncludes = false);
    Task InsertAsync(TEntity entity);
    Task InsertRangeAsync(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    IQueryable<TEntity> GetQueryable();
}
