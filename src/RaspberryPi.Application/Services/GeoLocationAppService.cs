using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Common;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Application.Services
{
    public sealed class GeoLocationAppService : IGeoLocationAppService
    {
        private readonly IGeoLocationInfraService _geoLocationService;

        public GeoLocationAppService(IGeoLocationInfraService geoLocationService)
        {
            _geoLocationService = geoLocationService;
        }

        public async Task<LookUpInfraDto> LookUpAsync(string ipAddress)
        {
            ArgumentException.ThrowIfNullOrEmpty(ipAddress);
            return await _geoLocationService.LookUpAsync(ipAddress);
        }

        public async Task<LookUpInfraDto> LookUpFromRandomIpAddressAsync()
        {
            var ipAddress = IPAddressHelper.GenerateRandomIPAddress();
            return await _geoLocationService.LookUpAsync(ipAddress.ToString());
        }
    }
}