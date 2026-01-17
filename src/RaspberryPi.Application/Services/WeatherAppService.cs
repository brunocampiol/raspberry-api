using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Extensions;
using RaspberryPi.Domain.Helpers;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.GeoLocation;
using RaspberryPi.Infrastructure.Models.Weather;

namespace RaspberryPi.Application.Services;

public sealed class WeatherAppService : IWeatherAppService
{
    private readonly IWeatherInfraService _weatherInfraService;
    private readonly IGeoLocationRepository _geoLocationRepository;
    private readonly IGeoLocationInfraService _geoLocationInfraService;
    private readonly IEmailAppService _emailAppService;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger _logger;

    public WeatherAppService(IWeatherInfraService watherInfraService,
                                IGeoLocationInfraService geoLocationInfraService,
                                IGeoLocationRepository geoLocationRepository,
                                IEmailAppService emailAppService,
                                IMemoryCache memoryCache,
                                ILogger<WeatherAppService> logger)
    {
        _weatherInfraService = watherInfraService;
        _geoLocationInfraService = geoLocationInfraService;
        _geoLocationRepository = geoLocationRepository;
        _emailAppService = emailAppService;
        _memoryCache = memoryCache;
        _logger = logger;
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
        var lookupResult = await GetCachedLookUpAsync(ipAddress.ToString());
        if (string.IsNullOrEmpty(lookupResult.CountryCode))
        {
            var message = "GeoPositioning CountryCode is null, empty or white-space characters " +
                          $"for ip address '{ipAddress}'. Response: '{lookupResult.ToJson()}'";
            _logger.LogWarning(message);
            return WeatherDto.NotAvailable();
        }

        // Fallback naming to avoid null values
        var parsedLocationName = lookupResult.City ?? lookupResult.RegionName ?? lookupResult.CountryName;
        if (string.IsNullOrWhiteSpace(parsedLocationName))
        {
            var message = "Unable to determine a city for ip address " +
                          $"'{ipAddress}'. Response: '{lookupResult.ToJson()}'";
            _logger.LogWarning(message);
            return WeatherDto.NotAvailable();
        }

        var geoLocation = new GeoLocation
        {
            CountryCode = lookupResult.CountryCode,
            LocationName = parsedLocationName,
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

        var lookupResult = await GetCachedLookUpAsync(ipAddress);
        if (string.IsNullOrEmpty(lookupResult.CountryCode))
        {
            var message = "GeoPositioning CountryCode is null, empty or white-space characters " +
                          $"for ip address '{ipAddress}'. Response: '{lookupResult.ToJson()}'";
            _logger.LogWarning(message);
            return WeatherDto.NotAvailable();
        }

        // Fallback naming to avoid null values
        var parsedLocationName = lookupResult.City ?? lookupResult.RegionName ?? lookupResult.CountryName;
        if (string.IsNullOrWhiteSpace(parsedLocationName))
        {
            var message = "Unable to determine a city for ip address " +
                          $"'{ipAddress}'. Response: '{lookupResult.ToJson()}'";
            _logger.LogWarning(message);
            return WeatherDto.NotAvailable();
        }
        
        var geoLocation = await _geoLocationRepository.GetAsync(lookupResult.CountryCode, 
                                                                lookupResult.Latitude, 
                                                                lookupResult.Longitude);

        if (geoLocation is null)
        {
            geoLocation = new GeoLocation
            {
                CountryCode = lookupResult.CountryCode,
                LocationName = parsedLocationName,
                Latitude = lookupResult.Latitude,
                Longitude = lookupResult.Longitude,
                CreatedAtUTC = DateTime.UtcNow
            };

            await _geoLocationRepository.AddAsync(geoLocation);
            var subject = $"New Geolocation {geoLocation.CountryCode} " +
                          $"{geoLocation.CountryCode.TryGetFlagEmoji()}";

            var email = new EmailDto
            {
                To = "bruno.campiol@gmail.com",
                Subject = subject,
                Body = geoLocation.ToJson()
            };

            await _emailAppService.TrySendEmailAsync(email);
        }

        var viewModel = await GetCachedWeatherDtoConditionsAsync(geoLocation);
        return viewModel;
    }

    private async Task<GeoLocationInfraResponse> GetCachedLookUpAsync(string ipAddress)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ipAddress);
        var cacheKey = $"Geolocation-{ipAddress}";

        if (!_memoryCache.TryGetValue(cacheKey, out GeoLocationInfraResponse? geoLocationDetails))
        {
            geoLocationDetails = await _geoLocationInfraService.LookUpAsync(ipAddress);

            // TODO use a configurable cache duration
            _memoryCache.Set(cacheKey, geoLocationDetails, TimeSpan.FromHours(12));
        }

        return geoLocationDetails ?? new GeoLocationInfraResponse { Ip = ipAddress };
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
            _memoryCache.Set(cacheKey, weatherDto, TimeSpan.FromMinutes(30));
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