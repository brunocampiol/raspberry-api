using Microsoft.EntityFrameworkCore;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Helpers;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Application.Services;

public sealed class GeoLocationAppService : IGeoLocationAppService
{
    private readonly IGeoLocationInfraService _geoLocationInfraService;
    private readonly IGeoLocationRepository _repository;

    public GeoLocationAppService(IGeoLocationInfraService geoLocationService,
                                IGeoLocationRepository repository)
    {
        _repository = repository;
        _geoLocationInfraService = geoLocationService;
    }

    public async Task<GeoLocationInfraResponse> LookUpAsync(string ipAddress)
    {
        ArgumentException.ThrowIfNullOrEmpty(ipAddress);
        return await _geoLocationInfraService.LookUpAsync(ipAddress);
    }

    public async Task<GeoLocationInfraResponse> LookUpFromRandomIpAddressAsync()
    {
        var ipAddress = RandomHelper.GenerateRandomIPAddress();
        return await _geoLocationInfraService.LookUpAsync(ipAddress.ToString());
    }

    public async Task<IEnumerable<GeoLocation>> GetAllGeoLocationsFromDatabaseAsync()
    {
        var dbResults = await _repository.GetAll().AsNoTracking().ToListAsync();
        return dbResults;
    }

    public async Task<int> ImportBackupAsync(IEnumerable<GeoLocation> geoLocations)
    {
        var existingIds = await _repository.GetAll()
                                    .Where(g => geoLocations.Select(gl => gl.Id).Contains(g.Id))
                                    .Select(g => g.Id)
                                    .ToListAsync();

        if (existingIds.Count > 0)
        {
            throw new InvalidOperationException($"There are already IDs: '{string.Join(", ", existingIds)}' in database");
        }

        await _repository.AddRangeAsync(geoLocations);
        await _repository.SaveChangesAsync();
        return geoLocations.Count();
    }
}