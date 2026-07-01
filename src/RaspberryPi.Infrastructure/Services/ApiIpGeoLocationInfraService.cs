using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Helpers;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models;
using RaspberryPi.Infrastructure.Models.GeoLocation;
using RaspberryPi.Infrastructure.Models.Options;
using System.Net.Http.Json;

namespace RaspberryPi.Infrastructure.Services;

// This class uses http://apiip.net/api/check?accessKey=KEY&ip=8.8.8.8

public class ApiIpGeoLocationInfraService : IGeoLocationProvider
{
    public string ProviderName => nameof(ApiIpGeoLocationInfraService);
    public bool IsAvailable => false; // TODO enable it back once ready

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
            var errorMessage = $"API IP response '{httpResponse.StatusCode}' " +
                              $"is not in 2XX range: '{httpContent}'";

            throw new AppException(errorMessage);
        }
        
        // TODO migrate to use example from new version
        var location = await httpResponse.Content.ReadFromJsonAsync<ApiIpNetResponse>(JsonDefaults.Options)
            ?? throw new AppException($"Unable to parse to {nameof(ApiIpNetResponse)} " +
                                      $"from content '{httpContent}'. Request IP is '{ipAddress}'");

        var validationErrors = new List<string>();
        if (string.IsNullOrWhiteSpace(location.CountryCode))
        {
            validationErrors.Add($"The country code is null, empty or consists of white-space characters: '{location.CountryCode}'");
        }
        if (string.IsNullOrWhiteSpace(location.CountryName))
        {
            validationErrors.Add($"The country name is null, empty or consists of white-space characters: '{location.CountryName}'");
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
            CountryCode = location.CountryCode!,
            LocationName = location.City ?? location.RegionName ?? location.CountryName!,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            PostalCode = location.PostalCode
        };
    }
}