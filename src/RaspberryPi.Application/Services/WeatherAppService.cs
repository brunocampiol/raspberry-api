using Microsoft.Extensions.Logging;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Domain.Services;
using RaspberryPi.Infrastructure.Interfaces;

namespace RaspberryPi.Application.Services
{
    public sealed class WeatherAppService : IWeatherAppService
    {
        private readonly IWeatherInfraService _weatherInfraService;
        private readonly IGeoLocationRepository _geoLocationRepository;
        private readonly IGeoLocationInfraService _geoLocationInfraService;
        private readonly ILogger _logger;

        public WeatherAppService(IWeatherInfraService watherInfraService,
                                    IGeoLocationInfraService geoLocationInfraService,
                                    IGeoLocationRepository geoLocationRepository,
                                    ILogger<WeatherAppService> logger)
        {
            _weatherInfraService = watherInfraService;
            _geoLocationInfraService = geoLocationInfraService;
            _geoLocationRepository = geoLocationRepository;
            _logger = logger;
        }

        public async Task<WeatherDto> GetWeatherFromIpAddress(string ipAddress)
        {
            var geoPositioning = await _geoLocationInfraService.LookUpAsync(ipAddress);

            if (string.IsNullOrEmpty(geoPositioning.CountryCode))
            {
                _logger.LogWarning("GeoPositioning CountryCode is null, empty or white-space characters");
                return WeatherDto.NotAvailable();
            }
            if (string.IsNullOrWhiteSpace(geoPositioning.PostalCode))
            {
                _logger.LogWarning("GeoPositioning PostalCode is null, empty or white-space characters");
                return WeatherDto.NotAvailable();
            }
            if (string.IsNullOrWhiteSpace(geoPositioning.City))
            {
                _logger.LogWarning("GeoPositioning City is null, empty or white-space characters");
                return WeatherDto.NotAvailable();
            }

            var geoLocation = await _geoLocationRepository.GetByPostalCodeAsync(geoPositioning.CountryCode, geoPositioning.PostalCode);
            if (geoLocation is null)
            {
                var accuWeatherLocations = await _weatherInfraService.PostalCodeSearch(geoPositioning.CountryCode, geoPositioning.PostalCode);
                if (accuWeatherLocations == null || !accuWeatherLocations.Any())
                {
                    _logger.LogWarning("AccuWeather location is null or empty result");
                    return WeatherDto.NotAvailable();
                }

                var location = accuWeatherLocations.First();
                geoLocation = new GeoLocation
                {
                    City = geoPositioning.City,
                    CountryCode = geoPositioning.CountryCode,
                    PostalCode = geoPositioning.PostalCode,
                    RegionName = geoPositioning.RegionName,
                    WeatherKey = location.Key
                };

                await _geoLocationRepository.AddAsync(geoLocation);
                await _geoLocationRepository.SaveChangesAsync();
            }

            var currentConditions = await _weatherInfraService.CurrentConditionsAsync(geoLocation.WeatherKey);
            var viewModel = new WeatherDto
            {
                EnglishName = geoPositioning.City,
                CountryCode = geoPositioning.CountryCode,
                WeatherText = currentConditions.First().WeatherText,
                Temperature = currentConditions.First().Temperature.Metric.Value + "°C"
            };

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
    }
}