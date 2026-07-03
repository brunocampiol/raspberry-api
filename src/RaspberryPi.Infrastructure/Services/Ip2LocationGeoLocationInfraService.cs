using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models;
using RaspberryPi.Infrastructure.Models.Options;
using System.Text.Json;

namespace RaspberryPi.Infrastructure.Services;

public class Ip2LocationGeoLocationInfraService : IGeoLocationProvider
{
    public string ProviderName => nameof(Ip2LocationGeoLocationInfraService);
    public bool IsAvailable => true;

    private readonly Ip2LocationGeoLocationOptions _settings;
    private readonly IHttpClientFactory _httpClientFactory;

    public Ip2LocationGeoLocationInfraService(IOptions<Ip2LocationGeoLocationOptions> settings,
                            IHttpClientFactory httpClientFactory)
    {
        _settings = settings.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<GeoLocationResult> GetGeoLocationAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(ipAddress);
        var httpClient = _httpClientFactory.CreateClient();
        var uri = new Uri($"{_settings.BaseUrl.OriginalString}/?key={_settings.APIKey}&ip={ipAddress}");

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

        var countryCode = root.GetProperty("country_code").GetString();
        var city = root.GetProperty("city_name").GetString();
        var region = root.GetProperty("region_name").GetString();
        var latitude = root.GetProperty("latitude").GetDouble();
        var longitude = root.GetProperty("longitude").GetDouble();
        var postalCode = root.GetProperty("zip_code").GetString();

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
