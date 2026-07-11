using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Extensions;
using RaspberryPi.Domain.Helpers;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Weather;

namespace RaspberryPi.Application.Services;

public sealed class WeatherAppService : IWeatherAppService
{
    private readonly IWeatherInfraService _weatherInfraService;
    private readonly IGeoLocationAppService _geoLocationAppService;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<WeatherAppService> _logger;

    public WeatherAppService(IWeatherInfraService watherInfraService,
                                IGeoLocationAppService geoLocationAppService,
                                IEmailAppService emailAppService,
                                IMemoryCache memoryCache,
                                ILogger<WeatherAppService> logger)
    {
        _weatherInfraService = watherInfraService ?? throw new ArgumentNullException(nameof(watherInfraService));
        _geoLocationAppService = geoLocationAppService ?? throw new ArgumentNullException(nameof(geoLocationAppService));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    /// <summary>
    /// Gets the raw current weather from infra service based on latitude and longitude. No caching.
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <returns></returns>
    /// <exception cref="AppException"></exception>
    public async Task<WeatherInfraResponse> GetInfraWeatherAsync(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
        {
            var errorMessage = $"Latitude must be between -90 and 90 degrees and not '{latitude}'";
            throw new ArgumentOutOfRangeException(nameof(latitude), errorMessage);
        }

        if (longitude < -180 || longitude > 180)
        {
            var errorMessage = $"Longitude must be between -180 and 180 degrees and not '{longitude}'";
            throw new ArgumentOutOfRangeException(nameof(longitude), errorMessage);
        }

        return await _weatherInfraService.CurrentAsync(latitude, longitude);
    }

    public async Task<WeatherDto> GetCurrentWeatherFromRandomIpAddressAsync()
    {
        var ipAddress = RandomHelper.GenerateRandomIPAddress();
        return await GetCurrentWeatherAsync(ipAddress.ToString());
    }

    public async Task<WeatherDto> GetCurrentWeatherAsync(string ipAddress)
    {
        var lookupResult = await _geoLocationAppService.GetCachedLookUpAsync(ipAddress.ToString());
        if (string.IsNullOrEmpty(lookupResult.CountryCode))
        {
            var message = "GeoPositioning CountryCode is null, empty or white-space characters " +
                          $"for ip address '{ipAddress}'. Response: '{lookupResult.ToJson()}'";
            _logger.LogWarning(message);
            return WeatherDto.NotAvailable();
        }

        // Fallback naming to avoid null values
        if (string.IsNullOrWhiteSpace(lookupResult.LocationName))
        {
            var message = "Unable to determine a location name for ip address " +
                          $"'{ipAddress}'. Response: '{lookupResult.ToJson()}'";
            _logger.LogWarning(message);
            return WeatherDto.NotAvailable();
        }

        var geoLocation = new GeoLocationEntity
        {
            CountryCode = lookupResult.CountryCode,
            LocationName = lookupResult.LocationName,
            Latitude = lookupResult.Latitude,
            Longitude = lookupResult.Longitude,
            CreatedAtUTC = DateTime.UtcNow
        };

        var infraWeather = await _weatherInfraService.CurrentAsync(geoLocation.Latitude, geoLocation.Longitude);
        if (infraWeather.Main is null)
        {
            var errorMessage = "Weather main data is null for coordinates: " +
                               $"({geoLocation.Latitude}, {geoLocation.Longitude})";
            throw new AppException(errorMessage);
        }

        var weatherDto = new WeatherDto()
        {
            EnglishName = geoLocation.LocationName,
            CountryCode = geoLocation.CountryCode,
            WeatherText = GetWeatherDescription(infraWeather),
            Temperature = $"{infraWeather.Main.Temperature:0.0} °C",
        };

        return weatherDto;
    }

    /// <summary>
    /// Retrieves weather information (may be cached) for a given IP address. Includes geolocation 
    /// lookup, database persistence, and email notifications for new locations. Called from the 
    /// brunocampiol.github.io main page.
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    public async Task<WeatherDto> GetWeatherWorkflowAsync(string ipAddress)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ipAddress);
        var geoLocation = await _geoLocationAppService.GetCachedLookUpAsync(ipAddress);

        if (string.IsNullOrEmpty(geoLocation.CountryCode))
        {
            var message = "GeoPositioning CountryCode is null, empty or white-space characters " +
                          $"for ip address '{ipAddress}'. Response: '{geoLocation.ToJson()}'";
            _logger.LogWarning(message);
            return WeatherDto.NotAvailable();
        }

        if (string.IsNullOrWhiteSpace(geoLocation.LocationName))
        {
            var message = "Unable to determine a location name for ip address " +
                          $"'{ipAddress}'. Response: '{geoLocation.ToJson()}'";
            _logger.LogWarning(message);
            return WeatherDto.NotAvailable();
        }
        
        var viewModel = await GetCachedWeatherDtoConditionsAsync(geoLocation);
        return viewModel;
    }
    
    private async Task<WeatherDto> GetCachedWeatherDtoConditionsAsync(GeoLocation geoLocation)
    {
        ArgumentNullException.ThrowIfNull(geoLocation);
        var cacheKey = $"Weather-{geoLocation.CountryCode}-{geoLocation.LocationName}";

        if (!_memoryCache.TryGetValue(cacheKey, out WeatherDto? weatherDto))
        {
            var infraWeather = await _weatherInfraService.CurrentAsync(geoLocation.Latitude, geoLocation.Longitude);
            if (infraWeather.Main is null)
            {
                var errorMessage = "Weather main data is null for coordinates: " +
                                   $"({geoLocation.Latitude}, {geoLocation.Longitude})";
                throw new AppException(errorMessage);
            }

            weatherDto = new WeatherDto()
            {
                EnglishName = geoLocation.LocationName,
                CountryCode = geoLocation.CountryCode,
                WeatherText = GetWeatherDescription(infraWeather),
                Temperature = $"{infraWeather.Main.Temperature:0.0} °C",
            };
             
            // TODO use a configurable cache duration
            _memoryCache.Set(cacheKey, weatherDto, TimeSpan.FromHours(24));
        }

        return weatherDto ?? WeatherDto.NotAvailable();
    }

    private static string GetWeatherDescription(WeatherInfraResponse weatherResponse)
    {
        const string noWeather = "No weather data available";

        if (weatherResponse?.Weather == null || weatherResponse.Weather.Length == 0)
        {
            return noWeather;
        }

        if (weatherResponse.Weather.Length == 1)
        {
            var firstWeather = weatherResponse.Weather[0].Description;
            if (string.IsNullOrWhiteSpace(firstWeather))
            {
                return noWeather;
            }

            return firstWeather.Trim().CapitalizeFirstLetter();
        }

        // For multiple descriptions, combine them
        var descriptions = weatherResponse.Weather.Select(w => w.Description).ToArray();
        var allDescriptions = string.Join(" and ", descriptions);
        return allDescriptions.Trim().CapitalizeFirstLetter();
    }

}