using Microsoft.EntityFrameworkCore;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Common;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Application.Services
{
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

        public async Task<LookUpInfraDto> LookUpAsync(string ipAddress)
        {
            ArgumentException.ThrowIfNullOrEmpty(ipAddress);
            return await _geoLocationInfraService.LookUpAsync(ipAddress);
        }

        public async Task<LookUpInfraDto> LookUpFromRandomIpAddressAsync()
        {
            var ipAddress = IPAddressHelper.GenerateRandomIPAddress();
            return await _geoLocationInfraService.LookUpAsync(ipAddress.ToString());
        }

        public async Task<IEnumerable<GeoLocation>> GetAllGeoLocationsFromDatabaseAsync()
        {
            var dbResults = await _repository.GetAll().AsNoTracking().ToListAsync();
            return dbResults;
        }

        public async Task<int> ImportBackupAsync(IEnumerable<GeoLocation> geoLocations)
        {
            foreach (var fact in geoLocations)
            {
                var dbFact = await _repository.GetByIdAsync(fact.Id);
                if (dbFact is not null)
                {
                    throw new InvalidOperationException($"There is already a fact ID '{dbFact.Id}' in database");
                }
            }

            // TODO: add range instead
            foreach (var fact in geoLocations)
            {
                await _repository.AddAsync(fact);
            }

            await _repository.SaveChangesAsync();
            return geoLocations.Count();
        }
    }
}