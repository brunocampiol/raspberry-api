using System.Linq.Expressions;

namespace RaspberryPi.Domain.Core;

public interface IRepository<T> : IDisposable where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void Remove(Guid id);
    Task RemoveAllAsync();
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);
    Task<int> SaveChangesAsync();
}