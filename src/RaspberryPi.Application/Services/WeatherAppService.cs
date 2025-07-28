using Fetchgoods.Text.Json.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Domain.Services;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.GeoLocation;

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

        public async Task<WeatherDto> GetWeatherFromIpAddress(string ipAddress)
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
                var accuWeatherLocations = await _weatherInfraService.PostalCodeSearch(lookupResult.CountryCode, lookupResult.PostalCode);
                if (accuWeatherLocations == null || !accuWeatherLocations.Any())
                {
                    _logger.LogWarning("AccuWeather location is null or empty result");
                    return WeatherDto.NotAvailable();
                }

                var location = accuWeatherLocations.First();
                geoLocation = new GeoLocation
                {
                    City = lookupResult.City,
                    CountryCode = lookupResult.CountryCode,
                    PostalCode = lookupResult.PostalCode,
                    RegionName = lookupResult.RegionName,
                    WeatherKey = location.Key
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

            var viewModel = await GetCachedWeatherConditionsAsync(geoLocation);
            return viewModel;
        }

        public async Task<WeatherDto> GetWeatherFromRandomIpAddressAsync()
        {
            var ipAddress = RandomService.GenerateRandomIPAddress();
            var location = await _weatherInfraService.LocationIpAddressSearchAsync(ipAddress.ToString());
            var currentConditions = await _weatherInfraService.CurrentConditionsAsync(location.Key);

            var weatherDto = new WeatherDto
            {
                EnglishName = location.EnglishName,
                CountryCode = location.Country.ID,
                WeatherText = currentConditions.First().WeatherText,
                Temperature = currentConditions.First().Temperature.Metric.Value + "°C"
            };

            return weatherDto;
        }

        private async Task<LookUpInfraDto> GetCachedLookUpAsync(string ipAddress)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(ipAddress);
            var cacheKey = $"Geolocation-{ipAddress}";

            if (!_memoryCache.TryGetValue(cacheKey, out LookUpInfraDto? lookupResult))
            {
                lookupResult = await _geoLocationInfraService.LookUpAsync(ipAddress);

                // TODO use a configurable cache duration
                _memoryCache.Set(cacheKey, lookupResult, TimeSpan.FromHours(12));
            }

            return lookupResult ?? new LookUpInfraDto { Ip = ipAddress };
        }

        private async Task<WeatherDto> GetCachedWeatherConditionsAsync(GeoLocation geoLocation)
        {
            ArgumentNullException.ThrowIfNull(geoLocation);
            var cacheKey = $"Weather-{geoLocation.CountryCode}-{geoLocation.WeatherKey}";

            if (!_memoryCache.TryGetValue(cacheKey, out WeatherDto? weatherDto))
            {
                var currentConditions = await _weatherInfraService.CurrentConditionsAsync(geoLocation.WeatherKey);
                weatherDto = new WeatherDto
                {
                    EnglishName = geoLocation.City,
                    CountryCode = geoLocation.CountryCode,
                    WeatherText = currentConditions.First().WeatherText,
                    Temperature = currentConditions.First().Temperature.Metric.Value + "°C"
                };

                // TODO use a configurable cache duration
                _memoryCache.Set(cacheKey, weatherDto, TimeSpan.FromMinutes(30));
            }

            return weatherDto ?? WeatherDto.NotAvailable();
        }
    }
}