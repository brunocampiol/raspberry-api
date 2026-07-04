using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Interfaces;

public interface IGeoLocationAppService
{
    Task<GeoLocationResult> LookUpAsync(string ipAddress, CancellationToken cancellationToken = default);
    Task<GeoLocationResult> LookUpApiIpAsync(string ipAddress, CancellationToken cancellationToken = default);
    Task<GeoLocationResult> LookUpFreeIpAsync(string ipAddress, CancellationToken cancellationToken = default);
    Task<GeoLocationResult> LookUpIp2LocationAsync(string ipAddress, CancellationToken cancellationToken = default);
    Task<GeoLocationResult> LookUpIpGeoAsync(string ipAddress, CancellationToken cancellationToken = default);
    Task<GeoLocationResult> LookUpFromRandomIpAddressAsync();
    Task<IEnumerable<GeoLocation>> GetAllGeoLocationsFromDatabaseAsync();
    Task<int> ImportBackupAsync(IEnumerable<GeoLocation> geoLocations);
}