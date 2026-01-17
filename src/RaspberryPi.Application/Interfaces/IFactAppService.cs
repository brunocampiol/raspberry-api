using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Application.Interfaces;

public interface IFactAppService
{
    Task<Fact?> GetFirstOrDefaultFactAsync(CancellationToken cancellationToken = default);
    Task<FactInfraDto> FetchFactAsync(CancellationToken cancellationToken = default);
    Task<FactInfraDto> FetchAndStoreUniqueFactAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Fact>> GetAllFactsAsync(CancellationToken cancellationToken = default);
    Task<long> CountAllFactsAsync(CancellationToken cancellationToken = default);
    Task<int> ImportBackupAsync(IEnumerable<Fact> facts, CancellationToken cancellationToken = default);
}