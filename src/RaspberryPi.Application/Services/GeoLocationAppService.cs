using RaspberryPi.Application.Interfaces;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.GeoLocation;
using System.Net;

namespace RaspberryPi.Application.Services
{
    public sealed class GeoLocationAppService : IGeoLocationAppService
    {
        private readonly IGeoLocationService _geoLocationService;

        public GeoLocationAppService(IGeoLocationService geoLocationService)
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
            var random = new Random();
            byte[] ipAddressBytes = new byte[4];
            random.NextBytes(ipAddressBytes);
            ipAddressBytes[0] = (byte)random.Next(1, 256); // Valid range for first octet is 1-255
            var ipAddress = new IPAddress(ipAddressBytes);
            return await _geoLocationService.LookUpAsync(ipAddress.ToString());
        }
    }
}