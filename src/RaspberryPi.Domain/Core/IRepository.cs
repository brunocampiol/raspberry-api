namespace RaspberryPi.Domain.Core
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(Guid id);
        Task RemoveAllAsync();
        IQueryable<T> GetAll();
        Task<int> SaveChangesAsync();
    }
}