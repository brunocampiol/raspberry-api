using Fetchgoods.Text.Json.Extensions;
using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Options;
using RaspberryPi.Infrastructure.Models.Weather;

namespace RaspberryPi.Infrastructure.Services;

// This class depends on https://openweathermap.org/ services
// APIs reference https://openweathermap.org/current
public class WeatherInfraService : IWeatherInfraService
{
    private readonly WeatherOptions _settings;
    private readonly IHttpClientFactory _httpClientFactory;

    public WeatherInfraService(IOptions<WeatherOptions> settings,
                               IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
    }

    public async Task<WeatherInfraResponse> CurrentAsync(double latitude, double longitude)
    {
        var endpoint = $"data/2.5/weather?lat={latitude}&lon={longitude}&units=metric&appid={_settings.ApiKey}";
        var httpClient = _httpClientFactory.CreateClient();
        var uri = new Uri($"{_settings.BaseUrl}{endpoint}");

        var httpResponse = await httpClient.GetAsync(uri);
        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = $"Failed to get PostalCodeSearch. " +
                               $"The HTTP response '{httpResponse.StatusCode}' " +
                               $"is not in 2XX range for '{uri}'. Received " +
                               $"content is '{httpContent}'";
            throw new AppException(errorMessage);
        }

        var result = httpContent.FromJsonTo<WeatherInfraResponse>()
              ?? throw new AppException($"Weather data deserialization failed. " +
                                        $"Coordinates: ({latitude}, {longitude}), " +
                                        $"HTTP content: {httpContent}");

        return result;
    }
}