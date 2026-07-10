using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Application.Interfaces;

public interface IFactAppService
{
    Task<FactEntity?> GetFirstOrDefaultFactAsync(CancellationToken cancellationToken = default);
    Task<FactInfraResponse> FetchFactAsync(CancellationToken cancellationToken = default);
    Task<FactInfraResponse> FetchAndStoreUniqueFactAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<FactEntity>> GetAllFactsAsync(CancellationToken cancellationToken = default);
    Task<long> CountAllFactsAsync(CancellationToken cancellationToken = default);
    Task<int> ImportBackupAsync(IEnumerable<FactEntity> facts, CancellationToken cancellationToken = default);
    Task<PagedResult<FactEntity>> SearchAsync(FactQuery query, CancellationToken cancellationToken = default);
}