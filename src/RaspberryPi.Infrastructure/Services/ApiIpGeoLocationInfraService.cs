using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models;
using RaspberryPi.Infrastructure.Models.Options;
using System.Text.Json;

namespace RaspberryPi.Infrastructure.Services;

// This class uses http://apiip.net/api/check?accessKey=KEY&ip=8.8.8.8

public class ApiIpGeoLocationInfraService : IGeoLocationProvider
{
    public string ProviderName => nameof(ApiIpGeoLocationInfraService);
    public bool IsAvailable => true;

    private readonly GeoLocationOptions _settings;
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiIpGeoLocationInfraService(IOptions<GeoLocationOptions> settings,
                                IHttpClientFactory httpClientFactory)
    {
        _settings = settings.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<GeoLocationResult> GetGeoLocationAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(ipAddress);
        var httpClient = _httpClientFactory.CreateClient();
        var uri = new Uri($"{_settings.BaseUrl.OriginalString}/api/check?accessKey={_settings.APIKey}&ip={ipAddress}");

        var httpResponse = await httpClient.GetAsync(uri);
        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = $"Vendor '{ProviderName}' http response for IP '{ipAddress}' is " +
                               $"not in 2XX range: '{httpResponse.StatusCode}' --> '{httpContent}'";
            throw new AppException(errorMessage);
        }

        using var stream = await httpResponse.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        var root = doc.RootElement;

        var countryCode = root.GetProperty("countryCode").GetString();
        var region = root.GetProperty("regionName").GetString();
        var latitude = root.GetProperty("latitude").GetDouble();
        var longitude = root.GetProperty("longitude").GetDouble();

        var validationErrors = new List<string>();
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            validationErrors.Add($"The 'countryCode' is null, empty or consists of white-space characters.");
        }
        if (string.IsNullOrWhiteSpace(region))
        {
            validationErrors.Add($"The 'region' is null, empty or consists of white-space characters.");
        }
        if (validationErrors.Count > 0)
        {
            var errorMessge = $"Given IP address '{ipAddress}', the returned data is missing " +
                              $"'{string.Join('.', validationErrors)}'. The returned content is {httpContent}";
            throw new AppException(errorMessge);
        }

        return new GeoLocationResult
        {
            Provider = ProviderName,
            CountryCode = countryCode!,
            LocationName = region!,
            Latitude = latitude,
            Longitude = longitude
        };
    }
}