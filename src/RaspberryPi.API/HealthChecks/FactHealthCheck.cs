using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Helpers;
using RaspberryPi.Infrastructure.Models.Facts;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.API.HealthChecks;

public class FactHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly FactOptions _settings;

    public FactHealthCheck(HttpClient httpClient, IOptions<FactOptions> settings)
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
            var path = $"api/v2/facts/random";
            var uri = new Uri($"{_settings.BaseUrl}{path}");

            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                                    cancellationToken,
                                    timeoutCts.Token);

            try
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
                var response = await _httpClient.SendAsync(httpRequest, linkedCts.Token);
                if (!response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Unhealthy(
                        $"{_settings.BaseUrl} returned {(int)response.StatusCode} " +
                        $"{response.ReasonPhrase}. Content: " +
                        $"{await response.Content.ReadAsStringAsync(linkedCts.Token)}");
                }

                var httpContent = await response.Content.ReadAsStringAsync();

                var result = await response.Content
                                .ReadFromJsonAsync<FactInfraResponse>(
                                    JsonDefaults.Options,
                                    cancellationToken);

                if (result is null)
                {
                    return HealthCheckResult.Unhealthy($"Invalid response content: '{httpContent}'");
                }

                return HealthCheckResult.Healthy();
            }
            catch (TaskCanceledException ex) when (timeoutCts.IsCancellationRequested)
            {
                return HealthCheckResult.Unhealthy($"Geolocation health check timed out after 20 seconds.", ex);
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Geolocation health check exception", ex);
        }
    }
}