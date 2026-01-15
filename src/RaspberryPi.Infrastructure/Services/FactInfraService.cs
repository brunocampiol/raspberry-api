using Fetchgoods.Text.Json.Extensions;
using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Facts;
using RaspberryPi.Infrastructure.Models.Options;

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

    public async Task<FactInfraDto> GetRandomFactAsync(CancellationToken cancellationToken = default)
    {
        var path = $"v1/facts";
        var uri = new Uri($"{_settings.BaseUrl}{path}");

        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                                cancellationToken,
                                timeoutCts.Token);

        var httpClient = _httpClientFactory.CreateClient();

        var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
        httpRequest.Headers.Add("X-Api-Key", _settings.APIKey);

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

        var result = httpContent.FromJsonTo<IEnumerable<FactInfraDto>>()
                                .First();

        return result;
    }
}