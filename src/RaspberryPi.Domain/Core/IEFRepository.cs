namespace RaspberryPi.Domain.Core
{
    public interface IEFRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        public void Add(T entity);
        public void Update(T entity);
        public void Remove(T entity);
        public T? GetById(Guid id);
        public IEnumerable<T> GetList();
    }
}