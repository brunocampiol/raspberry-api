using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Helpers;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Services;

namespace RaspberryPi.Application.Services;

public sealed class GeoLocationAppService : IGeoLocationAppService
{
    private readonly IGeoLocationProviderSelector _selector;
    private readonly IGeoLocationProvider _infraService;
    private readonly IGeoLocationRepository _repository;
    private readonly IEnumerable<IGeoLocationProvider> _providers;

    public GeoLocationAppService(
            IGeoLocationProviderSelector selector,
            IGeoLocationRepository repository,
            IEnumerable<IGeoLocationProvider> providers)
    {
        _selector = selector ?? throw new ArgumentNullException(nameof(selector));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _infraService = _selector.GetNextAvailableProvider() ?? 
            throw new InvalidOperationException("no geolocation service provider available");
        _providers = providers ?? throw new ArgumentNullException(nameof(providers));
    }

    public async Task<GeoLocationResult> LookUpAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        return await _infraService.GetGeoLocationAsync(ipAddress, cancellationToken);
    }

    public async Task<GeoLocationResult> LookUpApiIpAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        var providerName = nameof(ApiIpGeoLocationInfraService);
        var provider = _providers.FirstOrDefault(p => p.ProviderName == providerName);
        if (provider is null) throw new InvalidOperationException($"Provider '{providerName}' not found");
        return await provider.GetGeoLocationAsync(ipAddress, cancellationToken);
    }

    public async Task<GeoLocationResult> LookUpFreeIpAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        var providerName = nameof(FreeIpApiGeoLocationInfraService);
        var provider = _providers.FirstOrDefault(p => p.ProviderName == providerName);
        if (provider is null) throw new InvalidOperationException($"Provider '{providerName}' not found");
        return await provider.GetGeoLocationAsync(ipAddress, cancellationToken);
    }

    public async Task<GeoLocationResult> LookUpIp2LocationAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        var providerName = nameof(Ip2LocationGeoLocationInfraService);
        var provider = _providers.FirstOrDefault(p => p.ProviderName == providerName);
        if (provider is null) throw new InvalidOperationException($"Provider '{providerName}' not found");
        return await provider.GetGeoLocationAsync(ipAddress, cancellationToken);
    }

    public async Task<GeoLocationResult> LookUpIpGeoAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        var providerName = nameof(IpGeoLocationInfraService);
        var provider = _providers.FirstOrDefault(p => p.ProviderName == providerName);
        if (provider is null) throw new InvalidOperationException($"Provider '{providerName}' not found");
        return await provider.GetGeoLocationAsync(ipAddress, cancellationToken);
    }

    public async Task<GeoLocationResult> LookUpFromRandomIpAddressAsync()
    {
        var ipAddress = RandomHelper.GenerateRandomIPAddress();
        return await _infraService.GetGeoLocationAsync(ipAddress.ToString());
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