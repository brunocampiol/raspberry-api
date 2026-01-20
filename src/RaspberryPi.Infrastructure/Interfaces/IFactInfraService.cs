using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Infrastructure.Interfaces;

public interface IFactInfraService
{
    Task<FactInfraResponse> GetRandomFactAsync(CancellationToken cancellationToken = default);
}