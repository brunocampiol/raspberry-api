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

    public async Task<GeoLocationResult> GetGeoLocationAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(ipAddress);
        var httpClient = _httpClientFactory.CreateClient();
        var uri = new Uri($"{_settings.BaseUrl.OriginalString}/v3/ipgeo?apiKey={_settings.APIKey}&ip={ipAddress}");

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

        var location = root.GetProperty("location");
        var countryCode = root.GetProperty("location").GetProperty("country_code2").GetString();
        var city = root.GetProperty("location").GetProperty("city").GetString();
        var district = root.GetProperty("location").GetProperty("district").GetString();
        var latitude = root.GetProperty("location").GetProperty("latitude").GetString();
        var longitude = root.GetProperty("location").GetProperty("longitude").GetString();
        var postalCode = root.GetProperty("location").GetProperty("zipcode").GetString();

        var validationErrors = new List<string>();

        // TODO add validation to the location object
        //if (location. is null)
        //{
        //    validationErrors.Add("The location data is null or missing.");
        //}
        
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            validationErrors.Add($"The country code is null, empty or consists of white-space characters: '{countryCode}'");
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

        return new GeoLocationResult
        {
            Provider = ProviderName,
            CountryCode = countryCode!,
            LocationName = city ?? district ?? countryCode!,
            Latitude = lat,
            Longitude = lon,
            PostalCode = postalCode
        };
    }
}