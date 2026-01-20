using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Helpers;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Facts;
using RaspberryPi.Infrastructure.Models.Options;
using System.Net.Http.Json;

namespace RaspberryPi.Infrastructure.Services;

public sealed class FactInfraService : IFactInfraService
{
    private readonly FactOptions _settings;
    private readonly IHttpClientFactory _httpClientFactory;

    public FactInfraService(IOptions<FactOptions> settings,
                            IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
    }

    public async Task<FactInfraResponse> GetRandomFactAsync(CancellationToken cancellationToken = default)
    {
        var path = "api/v2/facts/random?language=en";
        var uri = new Uri($"{_settings.BaseUrl}{path}");

        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                                cancellationToken,
                                timeoutCts.Token);

        var httpClient = _httpClientFactory.CreateClient();
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);

        var httpResponse = await httpClient.SendAsync(httpRequest, linkedCts.Token);
        var httpContent = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = $"Failed to get GetRandomFactAsync. " +
                               $"The HTTP response '{httpResponse.StatusCode}' " +
                               $"is not in 2XX range for '{uri}'. Received " +
                               $"content is '{httpContent}'";
            throw new AppException(errorMessage);
        }

        var result = await httpResponse.Content
                            .ReadFromJsonAsync<FactInfraResponse>(
                                JsonDefaults.Options,
                                cancellationToken);

        if (result is null)
        {
            throw new AppException($"Failed to deserialize the fact response or the response is empty: '{httpContent}'.");
        }

        return result;
    }
}