using Fetchgoods.Text.Json.Extensions;
using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.GeoLocation;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.Infrastructure.Services
{
    // This class uses https://apiip.net/ service
    public class GeoLocationService : IGeoLocationService
    {
        private readonly GeoLocationOptions _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public GeoLocationService(IOptions<GeoLocationOptions> settings,
                                    IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<LookUpResponse> LookUpAsync(string ipAddress)
        {
            ArgumentException.ThrowIfNullOrEmpty(ipAddress);
            const string endpoint = "api/check";
            var httpClient = _httpClientFactory.CreateClient();
            var uri = new Uri($"{_settings.BaseUrl}{endpoint}?accessKey={_settings.APIKey}&ip={ipAddress}");

            var httpResponse = await httpClient.GetAsync(uri);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorMessage = $"API IP response '{httpResponse.StatusCode}' " +
                                  $"is not in 2XX range: '{httpContent}'";

                throw new AppException(errorMessage);
            }

            var result = httpContent.FromJsonTo<LookUpResponse>();
            return result;
        }
    }
}