

using buildingBlock.Entities;
using System.Linq.Expressions;

namespace buildingBlock.Repositories
{
    public interface IItemRepository<T> where T:IEntity 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetAsync(Guid Id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task RemoveAsync(Guid Id);
    }
}
