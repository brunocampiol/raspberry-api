using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Infrastructure.Interfaces;

public interface IFactInfraService
{
    Task<FactInfraDto> GetRandomFactAsync(CancellationToken cancellationToken = default);
}