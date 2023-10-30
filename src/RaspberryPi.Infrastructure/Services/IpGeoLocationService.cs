using Fetchgoods.Text.Json.Extensions;
using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.IpGeolocation;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.Infrastructure.Services
{
    public class IpGeoLocationService : IIpGeoLocationService
    {
        private readonly IpGeoLocationOptions _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public IpGeoLocationService(IOptions<IpGeoLocationOptions> settings,
                                    IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IpGeoLocationLookup> LookUp(string ipAddress)
        {
            ArgumentException.ThrowIfNullOrEmpty(ipAddress);
            const string endpoint = "ipgeo";
            var httpClient = _httpClientFactory.CreateClient();
            var uri = new Uri($"{_settings.BaseUrl}{endpoint}?apiKey={_settings.APIKey}&ip={ipAddress}");

            var httpResponse = await httpClient.GetAsync(uri);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorMessage = $"Failed to get IpGeolocationLookup. " +
                                   $"The HTTP response '{httpResponse.StatusCode}' " +
                                   $"is not in 2XX range for '{uri}'. Received " +
                                   $"content is '{httpContent}'";
                throw new AppException(errorMessage);
            }

            var result = httpContent.FromJsonTo<IpGeoLocationLookup>();
            return result;
        }
    }
}