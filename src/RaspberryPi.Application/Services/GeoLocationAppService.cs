using Microsoft.Extensions.Logging;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Helpers;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Services;

public sealed class GeoLocationAppService : IGeoLocationAppService
{
    private readonly IGeoLocationProviderSelector _selector;
    private readonly IGeoLocationProvider _service;
    private readonly IGeoLocationRepository _repository;
    private readonly ILogger<GeoLocationAppService> _logger;

    public GeoLocationAppService(
            IGeoLocationProviderSelector selector,
            IGeoLocationRepository repository,
            ILogger<GeoLocationAppService> logger)
    {
        _selector = selector ?? throw new ArgumentNullException(nameof(selector));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _service = _selector.GetNextAvailableProvider() ?? 
            throw new InvalidOperationException("no geolocation service provider available");
    }

    // TODO add specific vendor responses for each service provider for troubleshooting

    public async Task<GeoLocationResult> LookUpAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        return await _service.GetGeoLocationAsync(ipAddress, cancellationToken);
    }

    public async Task<GeoLocationResult> LookUpFromRandomIpAddressAsync()
    {
        var ipAddress = RandomHelper.GenerateRandomIPAddress();
        return await _service.GetGeoLocationAsync(ipAddress.ToString());
    }

    public async Task<IEnumerable<GeoLocation>> GetAllGeoLocationsFromDatabaseAsync()
    {
        var dbResults = await _repository.GetAllAsync();
        return dbResults;
    }

    public async Task<int> ImportBackupAsync(IEnumerable<GeoLocation> geoLocations)
    {
        var geoLocationIds = geoLocations.Select(e => e.Id).ToList();
        var geoLocationsInDb = await _repository.GetAllAsync(g => geoLocationIds.Contains(g.Id));
        var existingIds = geoLocationsInDb.Select(e => e.Id).ToList();

        if (existingIds.Count > 0)
        {
            throw new InvalidOperationException(
                $"The following '{existingIds.Count}' IDs already " +
                $"exist in the database: {string.Join(", ", existingIds)}.");
        }

        await _repository.AddRangeAsync(geoLocations);
        return geoLocations.Count();
    }
}