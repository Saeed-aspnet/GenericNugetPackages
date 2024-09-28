using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NugetPackage.GenericRepository.Interfaces;

namespace NugetPackage.GenericRepository;
public sealed class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _context;
    private readonly Dictionary<Type, object> _repositories;
    private IDbContextTransaction _transaction;

    public UnitOfWork(TContext context)
    {
        _context = context;
        _repositories = [];
    }
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);

        if (_repositories.TryGetValue(type, out object? value))
            return ((Lazy<IRepository<TEntity>>)value).Value;

        var lazyRepository = new Lazy<IRepository<TEntity>>(() => new Repository<TEntity>(_context));
        _repositories[type] = lazyRepository;

        return lazyRepository.Value;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction is not null)
            throw new InvalidOperationException("A transaction is already in progress.");

        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction is null)
            throw new InvalidOperationException("No transaction in progress.");

        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction is null)
            throw new InvalidOperationException("No transaction in progress.");

        try
        {
            await _transaction.RollbackAsync();
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}