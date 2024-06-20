using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Domain.Interfaces.Repositories
{
    public interface IFactRepository : IRepository<Fact>
    {
        Task<bool> HashExistsAsync(string hashValue);
        Task<IEnumerable<Fact>> GetAllDatabaseFactsAsync();
        Task<Fact?> GetFirstOrDefaultAsync();
        Task<long> CountAllDatabaseFacts();
    }
}