using RaspberryPi.Domain.Models;

namespace RaspberryPi.Domain.Data
{
    public interface IRepository<T> : IDisposable where T : IEntity
    {
        IUnitOfWork UnitOfWork { get; }
    }
}