using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models;
using RaspberryPi.Infrastructure.Models.Options;
using System.Text.Json;

namespace RaspberryPi.Infrastructure.Services;

// This class uses https://api.ipgeolocation.io/v3/ipgeo?apiKey=KEY&ip=8.8.8.8

public class IpGeoLocationInfraService : IGeoLocationProvider
{
    public string ProviderName => nameof(IpGeoLocationInfraService);
    public bool IsAvailable => true;

    private readonly IpGeoLocationOptions _settings;
    private readonly IHttpClientFactory _httpClientFactory;

    public IpGeoLocationInfraService(IOptions<IpGeoLocationOptions> settings,
                                IHttpClientFactory httpClientFactory)
    {
        _settings = settings.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<GeoLocation> GetGeoLocationAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(ipAddress);
        var httpClient = _httpClientFactory.CreateClient();
        var uri = new Uri($"{_settings.BaseUrl.OriginalString}/v3/ipgeo?apiKey={_settings.APIKey}&ip={ipAddress}");

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

        if (!root.TryGetProperty("location", out var location))
        {
            var errorMessage = $"Given IP address '{ipAddress}' and vendor '{ProviderName}', " +
                               $"the returned data is missing 'location' property. The " +
                               $"returned content is {httpContent}";
            throw new AppException(errorMessage);
        }

        var countryCode = location.TryGetProperty("country_code2", out var cc) ? cc.GetString() : null;
        var city = location.TryGetProperty("city", out var cn) ? cn.GetString() : null;
        var region = location.TryGetProperty("district", out var rn) ? rn.GetString() : null;
        var latitude = location.TryGetProperty("latitude", out var la) ? la.GetString() : null;
        var longitude = location.TryGetProperty("longitude", out var lo) ? lo.GetString() : null;
        var postalCode = location.TryGetProperty("zipcode", out var zc) ? zc.GetString() : null;

        var validationErrors = new List<string>();

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            validationErrors.Add($"The 'countryCode' is null, empty or consists of white-space characters.");
        }
        if (Double.TryParse(latitude, out var lat) == false)
        {
            validationErrors.Add($"The latitude is not a valid double: '{latitude}'");
        }
        if (Double.TryParse(longitude, out var lon) == false)
        {
            validationErrors.Add($"The longitude is not a valid double: '{longitude}'");
        }

        if (validationErrors.Count > 0)
        {
            var errorMessge = $"Given IP address '{ipAddress}', the returned data is missing " +
                              $"'{string.Join('.', validationErrors)}'. The returned content is {httpContent}";
            throw new AppException(errorMessge);
        }

        return new GeoLocation
        {
            Provider = ProviderName,
            CountryCode = countryCode!,
            LocationName = city ?? region ?? countryCode!,
            Latitude = lat,
            Longitude = lon,
            PostalCode = postalCode
        };
    }
}