using Fetchgoods.Text.Json.Extensions;
using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.IpGeolocation;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.Infrastructure.Services
{
    public class ApiIpService : IApiIpService
    {
        private readonly ApiIpOptions _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiIpService(IOptions<ApiIpOptions> settings,
                                    IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiIpCheck> Check(string ipAddress)
        {
            ArgumentException.ThrowIfNullOrEmpty(ipAddress);
            const string endpoint = "api/check";
            var httpClient = _httpClientFactory.CreateClient();
            var uri = new Uri($"{_settings.BaseUrl}{endpoint}?accessKey={_settings.APIKey}&ip={ipAddress}");

            var httpResponse = await httpClient.GetAsync(uri);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorMessage = $"Failed to get API IP check. " +
                                   $"The HTTP response '{httpResponse.StatusCode}' " +
                                   $"is not in 2XX range for '{uri}'. Received " +
                                   $"content is '{httpContent}'";
                throw new AppException(errorMessage);
            }

            var result = httpContent.FromJsonTo<ApiIpCheck>();
            return result;
        }
    }
}