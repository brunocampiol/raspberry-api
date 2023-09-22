using RaspberryPi.API.Models.Data;

namespace RaspberryPi.API.Repositories
{
    public interface IAspNetUserRepository : IRepository
    {
        void Add(AspNetUser AspNetUser);
        AspNetUser? GetNoTracking(Guid id);
        IEnumerable<AspNetUser> ListNoTracking();
    }
}