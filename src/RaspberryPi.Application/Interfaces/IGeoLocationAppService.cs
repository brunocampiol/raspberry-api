using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Application.Interfaces
{
    public interface IGeoLocationAppService
    {
        Task<LookUpInfraDto> LookUpAsync(string ipAddress);

        Task<LookUpInfraDto> LookUpFromRandomIpAddressAsync();
        Task<IEnumerable<GeoLocation>> GetAllGeoLocationsFromDatabaseAsync();
        Task<int> ImportBackupAsync(IEnumerable<GeoLocation> geoLocations);
    }
}