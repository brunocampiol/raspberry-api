using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IGeoLocationService
    {
        Task<LookUpResponse> LookUp(string ipAddress);
    }
}