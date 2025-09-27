using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IGeoLocationInfraService
    {
        Task<GeoLocationInfraResponse> LookUpAsync(string ipAddress);
    }
}