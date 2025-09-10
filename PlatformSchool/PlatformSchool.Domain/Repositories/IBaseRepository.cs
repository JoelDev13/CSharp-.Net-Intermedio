

using PlatformSchool.Domain.Base;
using System.Linq.Expressions;

namespace PlatformSchool.Domain.Repositories
{
    public interface IBaseRepository<TEntity, TModel> where TEntity : class
    {
        Task<OperationResult<TModel>> GetAllAsync();
        Task<TModel?> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);

    }
}
