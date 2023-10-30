using RaspberryPi.Infrastructure.Models.IpGeolocation;

namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IIpGeoLocationService
    {
        Task<IpGeoLocationLookup> LookUp(string ipAddress);
    }
}