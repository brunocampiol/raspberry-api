using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models;
using RaspberryPi.Infrastructure.Models.Options;
using System.Text.Json;

namespace RaspberryPi.Infrastructure.Services;

// This class uses https://free.freeipapi.com/api/json/8.8.8.8

public class FreeIpApiGeoLocationInfraService : IGeoLocationProvider
{
    public string ProviderName => nameof(FreeIpApiGeoLocationInfraService);
    public bool IsAvailable => true;

    private readonly FreeIpApiGeoLocationOptions _settings;
    private readonly IHttpClientFactory _httpClientFactory;

    public FreeIpApiGeoLocationInfraService(IOptions<FreeIpApiGeoLocationOptions> settings,
                            IHttpClientFactory httpClientFactory)
    {
        _settings = settings.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<GeoLocationResult> GetGeoLocationAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(ipAddress);
        var httpClient = _httpClientFactory.CreateClient();
        var uri = new Uri($"{_settings.BaseUrl.OriginalString}/api/json/{ipAddress}");

        var httpResponse = await httpClient.GetAsync(uri, cancellationToken);
        var httpContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = $"Vendor '{ProviderName}' http response for IP '{ipAddress}' is " +
                               $"not in 2XX range: '{httpResponse.StatusCode}' --> '{httpContent}'";
            throw new AppException(errorMessage);
        }

        using var doc = JsonDocument.Parse(httpContent);
        var root = doc.RootElement;

        var countryCode = root.TryGetProperty("countryCode", out var cc) ? cc.GetString() : null;
        var city = root.TryGetProperty("cityName", out var cn) ? cn.GetString() : null;
        var region = root.TryGetProperty("regionName", out var rn) ? rn.GetString() : null;
        double? latitude = root.TryGetProperty("latitude", out var la) ? la.GetDouble() : null;
        double? longitude = root.TryGetProperty("longitude", out var lo) ? lo.GetDouble() : null;
        var postalCode = root.TryGetProperty("zipCode", out var zc) ? zc.GetString() : null;

        var validationErrors = new List<string>();

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            validationErrors.Add($"The 'countryCode' is null, empty or consists of white-space characters: '{countryCode}'");
        }
        if (!latitude.HasValue)
        {
            validationErrors.Add($"The 'latitude' is null.");
        }
        if (!longitude.HasValue)
        {
            validationErrors.Add($"The 'longitude' is null.");
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
            LocationName = city ?? region ?? countryCode!,
            Latitude = latitude!.Value,
            Longitude = longitude!.Value,
            PostalCode = postalCode
        };
    }
}