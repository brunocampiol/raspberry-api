using Fetchgoods.Text.Json.Extensions;
using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.AccuWeather;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.Infrastructure.Services
{
    public class AccuWeatherService : IAccuWeatherService
    {
        private readonly AccuWeatherOptions _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccuWeatherService(IOptions<AccuWeatherOptions> settings,
                                  IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
        }

        public async Task<AccuWeatherLocationResponse> LocationIpAddressSearchAsync(string ipAddress)
        {
            ArgumentException.ThrowIfNullOrEmpty(ipAddress);
            const string endpoint = "locations/v1/cities/ipaddress";
            var httpClient = _httpClientFactory.CreateClient();
            var uri = new Uri($"{_settings.BaseUrl}{endpoint}?apikey={_settings.ApiKey}&q={ipAddress}");

            var httpResponse = await httpClient.GetAsync(uri);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorMessage = $"Failed to get LocationIpAddressSearchAsync. " +
                                   $"The HTTP response '{httpResponse.StatusCode}' " +
                                   $"is not in 2XX range for '{uri}'. Received " +
                                   $"content is '{httpContent}'";
                throw new AppException(errorMessage);
            }

            var result = httpContent.FromJsonTo<AccuWeatherLocationResponse>();
            return result;
        }

        public async Task<IEnumerable<AccuWeatherCurrentConditionsResponse>> CurrentConditionsAsync(string key)
        {
            ArgumentException.ThrowIfNullOrEmpty(key);
            const string endpoint = "currentconditions/v1/";
            var httpClient = _httpClientFactory.CreateClient();
            var uri = new Uri($"{_settings.BaseUrl}{endpoint}{key}?apikey={_settings.ApiKey}");

            var httpResponse = await httpClient.GetAsync(uri);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorMessage = $"Failed to get CurrentConditionsAsync. " +
                                   $"The HTTP response '{httpResponse.StatusCode}' " +
                                   $"is not in 2XX range for '{uri}'. Received " +
                                   $"content is '{httpContent}'";
                throw new AppException(errorMessage);
            }

            var result = httpContent.FromJsonTo<IEnumerable<AccuWeatherCurrentConditionsResponse>>();
            return result;
        }
    }
}
