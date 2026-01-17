using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Domain.Interfaces.Repositories;

public interface IFactRepository : IRepository<Fact>
{
    Task<bool> HashExistsAsync(string hashValue, CancellationToken cancellationToken = default);
    Task<Fact?> GetFirstOrDefaultAsync(CancellationToken cancellationToken = default);
    Task<long> CountAllDatabaseFacts(CancellationToken cancellationToken = default);
}