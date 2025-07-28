using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Application.Interfaces
{
    public interface IGeoLocationAppService
    {
        Task<IpGeoLocationInfraDetails> LookUpAsync(string ipAddress);

        Task<IpGeoLocationInfraDetails> LookUpFromRandomIpAddressAsync();
        Task<IEnumerable<GeoLocation>> GetAllGeoLocationsFromDatabaseAsync();
        Task<int> ImportBackupAsync(IEnumerable<GeoLocation> geoLocations);
    }
}