using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _memoryCache;
    private readonly IEnumerable<IGeoLocationProvider> _providers;

    public GeoLocationAppService(
            IGeoLocationProviderSelector selector,
            IGeoLocationRepository repository,
            IMemoryCache memoryCache,
            IEnumerable<IGeoLocationProvider> providers)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _providers = providers ?? throw new ArgumentNullException(nameof(providers));
        _selector = selector ?? throw new ArgumentNullException(nameof(selector));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _infraService = _selector.GetNextAvailableProvider() ?? 
            throw new InvalidOperationException("no geolocation service provider available");   
    }

    public async Task<GeoLocation> LookUpAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        return await _infraService.GetGeoLocationAsync(ipAddress, cancellationToken);
    }

    public async Task<GeoLocation> LookUpApiIpAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        var providerName = nameof(ApiIpGeoLocationInfraService);
        var provider = _providers.FirstOrDefault(p => p.ProviderName == providerName);
        if (provider is null) throw new InvalidOperationException($"Provider '{providerName}' not found");
        return await provider.GetGeoLocationAsync(ipAddress, cancellationToken);
    }

    public async Task<GeoLocation> LookUpFreeIpAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        var providerName = nameof(FreeIpApiGeoLocationInfraService);
        var provider = _providers.FirstOrDefault(p => p.ProviderName == providerName);
        if (provider is null) throw new InvalidOperationException($"Provider '{providerName}' not found");
        return await provider.GetGeoLocationAsync(ipAddress, cancellationToken);
    }

    public async Task<GeoLocation> LookUpIp2LocationAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        var providerName = nameof(Ip2LocationGeoLocationInfraService);
        var provider = _providers.FirstOrDefault(p => p.ProviderName == providerName);
        if (provider is null) throw new InvalidOperationException($"Provider '{providerName}' not found");
        return await provider.GetGeoLocationAsync(ipAddress, cancellationToken);
    }

    public async Task<GeoLocation> LookUpIpGeoAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        var providerName = nameof(IpGeoLocationInfraService);
        var provider = _providers.FirstOrDefault(p => p.ProviderName == providerName);
        if (provider is null) throw new InvalidOperationException($"Provider '{providerName}' not found");
        return await provider.GetGeoLocationAsync(ipAddress, cancellationToken);
    }

    public async Task<GeoLocation> LookUpFromRandomIpAddressAsync()
    {
        var ipAddress = RandomHelper.GenerateRandomIPAddress();
        return await _infraService.GetGeoLocationAsync(ipAddress.ToString());
    }

    public async Task<GeoLocation> GetCachedLookUpAsync(string ipAddress)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ipAddress);
        var cacheKey = $"Geolocation-{ipAddress}";

        if (!_memoryCache.TryGetValue(cacheKey, out GeoLocation? geoLocationDetails))
        {
            geoLocationDetails = await LookUpAsync(ipAddress);

            // TODO use a configurable cache duration
            _memoryCache.Set(cacheKey, geoLocationDetails, TimeSpan.FromHours(12));

            await InsertIntoDatabaseIfNewLocationAsync(geoLocationDetails);
        }        

        return geoLocationDetails ??
            throw new InvalidOperationException($"Failed to get cached geolocation details for IP address {ipAddress}");
    }

    public async Task<IReadOnlyList<string>> GetDistinctCountriesFromDatabaseAsync()
    {
        return await _repository.GetDistinctCountriesFromDatabaseAsync();
    }

    public async Task<IEnumerable<GeoLocationEntity>> GetAllGeoLocationsFromDatabaseAsync()
    {
        var dbResults = await _repository.GetAllAsync();
        return dbResults;
    }

    public async Task<int> ImportBackupAsync(IEnumerable<GeoLocationEntity> geoLocations)
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

    private async Task InsertIntoDatabaseIfNewLocationAsync(GeoLocation geoLocation)
    {
        ArgumentNullException.ThrowIfNull(geoLocation);

        var entity = await _repository.GetAsync(geoLocation.CountryCode,
                                                geoLocation.Latitude,
                                                geoLocation.Longitude);

        if (entity is null)
        {
            entity = new GeoLocationEntity
            {
                CountryCode = geoLocation.CountryCode,
                LocationName = geoLocation.LocationName,
                Latitude = geoLocation.Latitude,
                Longitude = geoLocation.Longitude,
                CreatedAtUTC = DateTime.UtcNow
            };

            await _repository.AddAsync(entity);
        }
    }
}