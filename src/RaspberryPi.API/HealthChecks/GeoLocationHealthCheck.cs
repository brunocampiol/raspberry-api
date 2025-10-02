using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.API.HealthChecks;

public class GeoLocationHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly GeoLocationOptions _settings;

    public GeoLocationHealthCheck(HttpClient httpClient, IOptions<GeoLocationOptions> settings)
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

            var url = new Uri($"{_settings.BaseUrl}api/check?accessKey={_settings.APIKey}&ip=8.8.8.8");
            using var response = await _httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Unhealthy(
                    $"IP API returned {(int)response.StatusCode} " +
                    $"{response.ReasonPhrase}. Content: " +
                    $"{await response.Content.ReadAsStringAsync(cancellationToken)}");
            }

            var json = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>(cancellationToken: cancellationToken);
            if (json is null || !json.ContainsKey("ip") || !json.ContainsKey("countryCode"))
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                return HealthCheckResult.Unhealthy($"Invalid payload response: '{responseContent}'");
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Geolocation health check exception", ex);
        }
    }
}
