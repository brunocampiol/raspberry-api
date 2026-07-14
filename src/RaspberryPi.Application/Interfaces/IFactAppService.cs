using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Interfaces;

public interface IFactAppService
{
    Task<FactEntity?> AddAsync(string factText, CancellationToken cancellationToken = default);
    Task<string?> GetRandomFactAsync(CancellationToken cancellationToken = default);
    Task<FactEntity?> GetFirstOrDefaultFactAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<FactEntity>> GetAllFactsAsync(CancellationToken cancellationToken = default);
    Task<long> CountAllFactsAsync(CancellationToken cancellationToken = default);
    Task<int> ImportBackupAsync(IEnumerable<FactEntity> facts, CancellationToken cancellationToken = default);
    Task<PagedResult<FactEntity>> SearchAsync(FactQuery query, CancellationToken cancellationToken = default);
}