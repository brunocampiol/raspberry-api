using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Models;

namespace RaspberryPi.Domain.Interfaces
{
    public interface IAspNetUserRepository : IRepository<AspNetUser>
    {
        void Add(AspNetUser AspNetUser);
        AspNetUser? GetNoTracking(Guid id);
        IEnumerable<AspNetUser> ListNoTracking();
    }
}