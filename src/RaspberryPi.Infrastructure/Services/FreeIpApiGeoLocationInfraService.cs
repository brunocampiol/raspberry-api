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

        var httpResponse = await httpClient.GetAsync(uri);
        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            // TODO: better message - review
            var errorMessage = $"HTTP response '{httpResponse.StatusCode}' " +
                               $"is not in 2XX range: '{httpContent}'";

            throw new AppException(errorMessage);
        }

        using var stream = await httpResponse.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        var root = doc.RootElement;

        var countryCode = root.GetProperty("countryCode").GetString();
        var city = root.GetProperty("cityName").GetString();
        var region = root.GetProperty("regionName").GetString();
        var latitude = root.GetProperty("latitude").GetDouble();
        var longitude = root.GetProperty("longitude").GetDouble();
        var postalCode = root.GetProperty("zipCode").GetString();

        var validationErrors = new List<string>();

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            validationErrors.Add($"The country code is null, empty or consists of white-space characters: '{countryCode}'");
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
            Latitude = latitude,
            Longitude = longitude,
            PostalCode = postalCode
        };
    }
}
