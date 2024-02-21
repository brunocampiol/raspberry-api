using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Domain.Interfaces.Repositories
{
    public interface IDapperRepository
    {
        bool Create(AspNetUser user);

        AspNetUser? Get(Guid id);

        IEnumerable<AspNetUser> List();

        bool Update(AspNetUser user);

        bool Delete(Guid id);

        int DeleteAll();
    }
}