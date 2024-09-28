using System.Linq.Expressions;

namespace NugetPackage.GenericRepository.Interfaces;
public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> FindByIdAsync(int id);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task BulkInsertAsync(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void BulkUpdateAsync(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
}