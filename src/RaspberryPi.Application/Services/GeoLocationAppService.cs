using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.GeoLocation;
using System.Net;

namespace RaspberryPi.Application.Services
{
    public sealed class GeoLocationAppService
    {
        private readonly IGeoLocationService _geoLocationService;

        public GeoLocationAppService(IGeoLocationService geoLocationService)
        {
            _geoLocationService = geoLocationService;
        }

        public async Task<LookUpResponse> LookUp(string ipAddress)
        {
            ArgumentException.ThrowIfNullOrEmpty(ipAddress);
            return await _geoLocationService.LookUp(ipAddress);
        }

        public async Task<LookUpResponse> LookUpFromRandomIpAddress()
        {
            var random = new Random();
            byte[] ipAddressBytes = new byte[4];
            random.NextBytes(ipAddressBytes);
            var ipAddress = new IPAddress(ipAddressBytes);
            return await _geoLocationService.LookUp(ipAddress.ToString());
        }
    }
}