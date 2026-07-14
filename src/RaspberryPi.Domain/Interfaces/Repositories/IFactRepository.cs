using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Domain.Interfaces.Repositories;

public interface IFactRepository : IRepository<FactEntity>
{
    Task<bool> HashExistsAsync(string hashValue, CancellationToken cancellationToken = default);
    Task<FactEntity?> GetFirstOrDefaultAsync(CancellationToken cancellationToken = default);
    Task<FactEntity?> GetRandomFactAsync(CancellationToken cancellationToken = default);
    Task<long> CountAllDatabaseFacts(CancellationToken cancellationToken = default);
    Task<PagedResult<FactEntity>> SearchAsync(FactQuery query, CancellationToken cancellationToken = default);
}