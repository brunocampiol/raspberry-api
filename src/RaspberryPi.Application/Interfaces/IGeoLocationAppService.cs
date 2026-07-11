using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Interfaces;

public interface IGeoLocationAppService
{
    Task<GeoLocation> LookUpAsync(string ipAddress, CancellationToken cancellationToken = default);
    Task<GeoLocation> LookUpApiIpAsync(string ipAddress, CancellationToken cancellationToken = default);
    Task<GeoLocation> LookUpFreeIpAsync(string ipAddress, CancellationToken cancellationToken = default);
    Task<GeoLocation> LookUpIp2LocationAsync(string ipAddress, CancellationToken cancellationToken = default);
    Task<GeoLocation> LookUpIpGeoAsync(string ipAddress, CancellationToken cancellationToken = default);
    Task<GeoLocation> LookUpFromRandomIpAddressAsync();
    Task<IReadOnlyList<string>> GetDistinctCountriesFromDatabaseAsync();
    Task<IEnumerable<GeoLocationEntity>> GetAllGeoLocationsFromDatabaseAsync();
    Task<int> ImportBackupAsync(IEnumerable<GeoLocationEntity> geoLocations);
}