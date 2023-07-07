using IRepository;
using IService;
using System.Linq.Expressions;

namespace Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        protected IBaseRepository<TEntity> _repository;

        public async Task AddAsync(TEntity entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
           await _repository.DeleteAsync(entity);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
           return await _repository.GetAllAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
           return await _repository.GetByIdAsync(id);
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func)
        {
           return await _repository.QueryAsync(func);
        }

        public async Task UpdateAsync(TEntity entity)
        {
           await _repository.UpdateAsync(entity);
        }
    }
}
