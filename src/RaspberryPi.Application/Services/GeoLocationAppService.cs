using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Common;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Application.Services
{
    public sealed class GeoLocationAppService : IGeoLocationAppService
    {
        private readonly IGeoLocationInfraService _geoLocationInfraService;

        public GeoLocationAppService(IGeoLocationInfraService geoLocationService)
        {
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
    }
}