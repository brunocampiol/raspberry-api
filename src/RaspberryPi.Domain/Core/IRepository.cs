namespace RaspberryPi.Domain.Core
{
    public interface IRepository<T> : IDisposable where T : class
    {
        //IUnitOfWork UnitOfWork { get; }
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(Guid id);
        IQueryable<T> GetAll();
        Task<int> SaveChangesAsync();
    }
}