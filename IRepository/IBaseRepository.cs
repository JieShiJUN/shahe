using System.Linq.Expressions;

namespace IRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class, new()
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func);

    }
}