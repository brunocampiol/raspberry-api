using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.API.HealthChecks;

public class WeatherHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly WeatherOptions _settings;

    public WeatherHealthCheck(HttpClient httpClient, IOptions<WeatherOptions> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url = new Uri(_settings.BaseUrl, $"data/2.5/weather?q=London&appid={_settings.ApiKey}&units=metric");
            using var response = await _httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Unhealthy(
                    $"OpenWeather API returned {(int)response.StatusCode} " +
                    $"{response.ReasonPhrase}. Content: " +
                    $"{await response.Content.ReadAsStringAsync(cancellationToken)}");
            }

            var json = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>(cancellationToken: cancellationToken);
            if (json is null || !json.ContainsKey("weather"))
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                return HealthCheckResult.Unhealthy($"Invalid payload response: '{responseContent}'");
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Weather health check exception", ex);
        }
    }
}