using Fetchgoods.Text.Json.Extensions;
using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.GeoLocation;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.Infrastructure.Services;

// This class uses https://apiip.net/ service

public class GeoLocationInfraService : IGeoLocationInfraService
{
    private readonly GeoLocationOptions _settings;
    private readonly IHttpClientFactory _httpClientFactory;

    public GeoLocationInfraService(IOptions<GeoLocationOptions> settings,
                                IHttpClientFactory httpClientFactory)
    {
        _settings = settings.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<GeoLocationInfraResponse> LookUpAsync(string ipAddress)
    {
        ArgumentException.ThrowIfNullOrEmpty(ipAddress);
        var httpClient = _httpClientFactory.CreateClient();
        var uri = new Uri($"{_settings.BaseUrl}api/check?accessKey={_settings.APIKey}&ip={ipAddress}");

        var httpResponse = await httpClient.GetAsync(uri);
        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = $"API IP response '{httpResponse.StatusCode}' " +
                              $"is not in 2XX range: '{httpContent}'";

            throw new AppException(errorMessage);
        }

        var location = httpContent.FromJsonTo<GeoLocationInfraResponse>()
            ?? throw new AppException($"Unable to parse to {nameof(GeoLocationInfraResponse)} " +
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
                              $"'{string.Join('.',validationErrors)}'. The returned content is {httpContent}";
            throw new AppException(errorMessge);
        }

        return location;
    }
}