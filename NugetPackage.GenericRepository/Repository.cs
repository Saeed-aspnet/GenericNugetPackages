using Microsoft.EntityFrameworkCore;
using NugetPackage.GenericRepository.Interfaces;
using System.Linq.Expressions;

namespace NugetPackage.GenericRepository;
public sealed class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context) => _dbSet = context.Set<TEntity>();

    public async Task<TEntity?> FindByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public async Task BulkInsertAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void BulkUpdateAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}