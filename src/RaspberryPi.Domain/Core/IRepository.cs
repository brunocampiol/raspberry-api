using System.Linq.Expressions;

namespace RaspberryPi.Domain.Core;

public interface IRepository<T> where T : IEntityBase
{
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task RemoveAsync(Guid id);
    Task RemoveAllAsync();
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);
}