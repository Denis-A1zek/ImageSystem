
namespace ImageSystem.Domain;

public interface IUnitOfWork
{
    Task<bool> Commit();
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : Identity;
    object GetRepository(Type type);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransaction();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
