using RaspberryPi.Domain.Models;

namespace RaspberryPi.Domain.Data.Repositories
{
    public interface IAspNetUserRepository : IRepository<AspNetUser>
    {
        void Add(AspNetUser AspNetUser);
        AspNetUser? GetNoTracking(Guid id);
        IEnumerable<AspNetUser> ListNoTracking();
    }
}