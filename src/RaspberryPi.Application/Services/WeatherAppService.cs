using Fetchgoods.Text.Json.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Helpers;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.GeoLocation;
using RaspberryPi.Infrastructure.Models.Weather;

namespace RaspberryPi.Application.Services
{
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

        public async Task<WeatherDto> CurrentWeatherFromRandomIpAddressAsync()
        {
            var ipAddress = RandomHelper.GenerateRandomIPAddress();
            return await CurrentWeatherFromIpAddress(ipAddress.ToString());
        }

        public async Task<WeatherDto> CurrentWeatherFromIpAddress(string ipAddress)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(ipAddress);

            var lookupResult = await GetCachedLookUpAsync(ipAddress);
            if (string.IsNullOrEmpty(lookupResult.CountryCode))
            {
                _logger.LogWarning("GeoPositioning CountryCode is null, empty or white-space characters");
                return WeatherDto.NotAvailable();
            }
            if (string.IsNullOrWhiteSpace(lookupResult.PostalCode))
            {
                _logger.LogWarning("GeoPositioning PostalCode is null, empty or white-space characters");
                return WeatherDto.NotAvailable();
            }
            if (string.IsNullOrWhiteSpace(lookupResult.City))
            {
                _logger.LogWarning("GeoPositioning City is null, empty or white-space characters");
                return WeatherDto.NotAvailable();
            }
            
            var geoLocation = await _geoLocationRepository.GetByPostalCodeAsync(lookupResult.CountryCode, lookupResult.PostalCode);
            if (geoLocation is null)
            {
                geoLocation = new GeoLocation
                {
                    City = lookupResult.City,
                    CountryCode = lookupResult.CountryCode,
                    PostalCode = lookupResult.PostalCode,
                    RegionName = lookupResult.RegionName,
                    Latitude = lookupResult.Latitude,
                    Longitude = lookupResult.Longitude,
                    CreatedAtUTC = DateTime.UtcNow
                };

                await _geoLocationRepository.AddAsync(geoLocation);
                await _geoLocationRepository.SaveChangesAsync();

                var email = new EmailDto
                {
                    To = "bruno.campiol@gmail.com",
                    Subject = $"New Geolocation {geoLocation.CountryCode}",
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
            var cacheKey = $"Weather-{geoLocation.CountryCode}-{geoLocation.PostalCode}";

            if (!_memoryCache.TryGetValue(cacheKey, out WeatherDto? weatherDto))
            {
                var infraWeather = await _weatherInfraService.CurrentAsync(geoLocation.Latitude, geoLocation.Longitude);
                weatherDto = new WeatherDto()
                {
                    EnglishName = infraWeather.CityName,
                    CountryCode = infraWeather.System.CountryCode,
                    WeatherText = GetWeatherDescription(infraWeather),
                    Temperature = infraWeather.Main.Temperature + "°C"
                };

                // TODO use a configurable cache duration
                _memoryCache.Set(cacheKey, weatherDto, TimeSpan.FromMinutes(30));
            }

            return weatherDto ?? WeatherDto.NotAvailable();
        }

        private static string GetWeatherDescription(WeatherInfraResponse weatherResponse)
        {
            if (weatherResponse?.Weather == null || weatherResponse.Weather.Length == 0)
            {
                return "No weather data available";
            }

            if (weatherResponse.Weather.Length == 1)
            {
                return weatherResponse.Weather[0].Description;
            }

            // For multiple descriptions, combine them naturally
            var descriptions = weatherResponse.Weather.Select(w => w.Description).ToArray();
            return string.Join(" and ", descriptions);
        }
    }
}