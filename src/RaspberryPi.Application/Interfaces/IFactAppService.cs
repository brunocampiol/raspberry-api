using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Application.Interfaces
{
    public interface IFactAppService
    {
        Task<Fact?> GetFirstOrDefaultFactAsync();
        Task<FactInfraDto> FetchFactAsync(CancellationToken cancellationToken = default);
        Task<FactInfraDto> FetchAndStoreUniqueFactAsync();
        Task<IEnumerable<Fact>> GetAllFactsAsync();
        Task<long> CountAllFactsAsync();
        Task<int> ImportBackupAsync(IEnumerable<Fact> facts);
    }
}