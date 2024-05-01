using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Domain.Interfaces.Repositories
{
    public interface IAspNetUserRepository : IRepository<AspNetUser>
    {
        //void Add(AspNetUser AspNetUser);
        AspNetUser? GetNoTracking(Guid id);
        IEnumerable<AspNetUser> ListNoTracking();
    }
}