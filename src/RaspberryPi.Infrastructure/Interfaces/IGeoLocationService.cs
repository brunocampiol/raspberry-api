using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IGeoLocationService
    {
        Task<LookUpInfraDto> LookUpAsync(string ipAddress);
    }
}