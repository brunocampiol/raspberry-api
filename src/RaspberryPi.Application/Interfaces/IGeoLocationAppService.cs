using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Application.Interfaces
{
    public interface IGeoLocationAppService
    {
        Task<GeoLocationInfraResponse> LookUpAsync(string ipAddress);

        Task<GeoLocationInfraResponse> LookUpFromRandomIpAddressAsync();
        Task<IEnumerable<GeoLocation>> GetAllGeoLocationsFromDatabaseAsync();
        Task<int> ImportBackupAsync(IEnumerable<GeoLocation> geoLocations);
    }
}