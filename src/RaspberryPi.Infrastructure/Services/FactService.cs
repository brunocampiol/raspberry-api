using Fetchgoods.Text.Json.Extensions;
using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Facts;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.Infrastructure.Services
{
    public sealed class FactService : IFactService
    {
        private readonly FactOptions _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public FactService(IOptions<FactOptions> settings,
                            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
        }

        public async Task<FactInfraDto> GetRandomFactAsync()
        {
            var endpoint = $"v1/facts";
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", _settings.APIKey);
            var uri = new Uri($"{_settings.BaseUrl}{endpoint}");
           
            var httpResponse = await httpClient.GetAsync(uri);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorMessage = $"Failed to get GetRandomFactAsync. " +
                                   $"The HTTP response '{httpResponse.StatusCode}' " +
                                   $"is not in 2XX range for '{uri}'. Received " +
                                   $"content is '{httpContent}'";
                throw new AppException(errorMessage);
            }

            var result = httpContent.FromJsonTo<IEnumerable<FactInfraDto>>().First();
            return result;
        }
    }
}
