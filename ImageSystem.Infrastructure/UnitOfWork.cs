using ImageSystem.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ImageSystem.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly PostgreContext _context;

    private Dictionary<string, object> _repositories;
    private IDbContextTransaction? _currentTransaction;
    private ILogger<IUnitOfWork> _logger;

    public UnitOfWork(PostgreContext context, ILogger<IUnitOfWork> logger)
        => (_context, _logger) = (context, logger);

    public async Task<bool> Commit()
        => await _context.SaveChangesAsync() > 0;

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Identity
    {
        if (_repositories == null)
            _repositories = new Dictionary<string, object>();

        var type = typeof(TEntity).Name;

        if (_repositories.ContainsKey(type))
            return (IRepository<TEntity>)_repositories[type];
        Type repositoryType = typeof(Repository<>).MakeGenericType(typeof(TEntity));

        ConstructorInfo constructor = repositoryType.GetConstructor(new[] { typeof(PostgreContext) });

        _repositories.Add(type, constructor.Invoke(new object[] { _context }));

        return (IRepository<TEntity>)_repositories[type];
    }

    public object GetRepository(Type type)
    {
        if (_repositories == null)
            _repositories = new Dictionary<string, object>();

        if (_repositories.ContainsKey(type.Name))
            return (IRepository<Identity>)_repositories[type.Name];
        Type repositoryType = typeof(Repository<>).MakeGenericType(type);

        ConstructorInfo constructor = repositoryType.GetConstructor(new[] { typeof(PostgreContext) });

        _repositories.Add(type.Name, constructor.Invoke(new object[] { _context }));

        return _repositories[type.Name];
    }

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction is not null)
        {
            _logger.LogInformation("A transaction with ID {ID} is already created", _currentTransaction.TransactionId);
            return;
        }

        _currentTransaction = await _context.Database.BeginTransactionAsync();
        _logger.LogInformation("A new transaction was created with ID {ID}", _currentTransaction.TransactionId);
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction is null)
        {
            return;
        }

        _logger.LogInformation("Commiting Transaction {ID}", _currentTransaction.TransactionId);

        await _currentTransaction.CommitAsync();

        _currentTransaction.Dispose();
        _currentTransaction = null;
    }

    public async Task RollbackTransaction()
    {
        if (_currentTransaction is null)
        {
            return;
        }

        _logger.LogDebug("Rolling back Transaction {ID}", _currentTransaction.TransactionId);

        await _currentTransaction.RollbackAsync();

        _currentTransaction.Dispose();
        _currentTransaction = null;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

}
