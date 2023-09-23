using RaspberryPi.API.Models.Data;

namespace RaspberryPi.API.Repositories
{
    public interface IRepository<T> : IDisposable where T : IEntity
    {
        //IUnitOfWork UnitOfWork { get; }
    }
}