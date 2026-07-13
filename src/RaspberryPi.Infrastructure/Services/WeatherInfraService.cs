using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Extensions;
using RaspberryPi.Domain.Models;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Options;
using System.Text.Json;

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

    public async Task<Weather> CurrentAsync(double latitude, double longitude)
    {
        var endpoint = $"data/2.5/weather?lat={latitude}&lon={longitude}&units=metric&appid={_settings.ApiKey}";
        var httpClient = _httpClientFactory.CreateClient();
        var uri = new Uri($"{_settings.BaseUrl}{endpoint}");

        var httpResponse = await httpClient.GetAsync(uri);
        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = $"Failed to get weather for lat '{latitude}' and lon '{longitude}'. " +
                               $"The HTTP response '{httpResponse.StatusCode}' is not in 2XX range for " +
                               $"'{uri}'. Received content is '{httpContent}'";
            throw new AppException(errorMessage);
        }

        using var doc = JsonDocument.Parse(httpContent);
        var root = doc.RootElement;


        if (!root.TryGetProperty("weather", out var weather))
        {
            var errorMessage = $"Failed to get weather property for lat '{latitude}' and lon '{longitude}'. " +
                               $"Returned content is {httpContent}";
            throw new AppException(errorMessage);
        }       

        if (!root.TryGetProperty("main", out var main))
        {
            var errorMessage = $"Failed to get main property for lat '{latitude}' and lon '{longitude}'. " +
                               $"Returned content is {httpContent}";
            throw new AppException(errorMessage);
        }

        double? temperature = main.TryGetProperty("temp", out var temp) ? temp.GetDouble() : null;

        var validationErrors = new List<string>();
        if (!temperature.HasValue)
        {
            validationErrors.Add($"The 'temperature' is null.");
        }

        if (validationErrors.Count > 0)
        {
            var errorMessage = $"Validation failed for lat '{latitude}' and lon '{longitude}'. " +
                               $"'{string.Join('.', validationErrors)}'. The returned content is {httpContent}";
            throw new AppException(errorMessage);
        }

        return new Weather
        {
            Latitude = latitude,
            Longitude = longitude,
            Description = GetWeatherDescription(weather),
            TemperatureCelcius = temperature!.Value
        };
    }

    private static string GetWeatherDescription(JsonElement element)
    {
        const string noWeather = "No weather data available";
        if (element.ValueKind != JsonValueKind.Array)
        {
            var errorMessage = $"Weather property is not an array. ValueKind: {element.ValueKind}";
            throw new AppException(errorMessage);
        }

        int weatherCount = element.GetArrayLength();

        if (weatherCount == 1)
        {
            var description = element[0].TryGetProperty("description", out var d) ? d.GetString() : null;
            return description?.Trim().CapitalizeFirstLetter() ?? noWeather;
        }

        if (weatherCount > 1)
        {
            var descriptions = new List<string>();
            for (int i = 0; i < weatherCount; i++)
            {
                if (element[i].TryGetProperty("description", out var d) && d.GetString() is { Length: > 0 } desc)
                {
                    descriptions.Add(desc.Trim());
                }
            }

            return descriptions.Count > 0 ? string.Join(" and ", descriptions).CapitalizeFirstLetter() : noWeather;
        }

        return noWeather;
    }
}