using Microsoft.Extensions.Logging;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Common;
using RaspberryPi.Infrastructure.Interfaces;

namespace RaspberryPi.Application.Services
{
    public sealed class WeatherAppService : IWeatherAppService
    {
        private readonly IWeatherInfraService _weatherService;
        private readonly IGeoLocationInfraService _geoLocationService;
        private readonly ILogger _logger;

        public WeatherAppService(IWeatherInfraService accuWeatherService,
                                    IGeoLocationInfraService apiIpService,
                                    ILogger<WeatherAppService> logger)
        {
            _weatherService = accuWeatherService;
            _geoLocationService = apiIpService;
            _logger = logger;
        }

        public async Task<WeatherDto> GetWeatherFromIpAddress(string ipAddress)
        {
            var geoPositioning = await _geoLocationService.LookUpAsync(ipAddress);

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

            var accuWeatherLocations = await _weatherService.PostalCodeSearch(geoPositioning.CountryCode, geoPositioning.PostalCode);
            if (accuWeatherLocations == null || !accuWeatherLocations.Any())
            {
                _logger.LogWarning("AccuWeather location is null or empty result");
                return WeatherDto.NotAvailable();
            }

            var location = accuWeatherLocations.First();
            var currentConditions = await _weatherService.CurrentConditionsAsync(location.Key);

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
            var ipAddress = IPAddressHelper.GenerateRandomIPAddress();
            var location = await _weatherService.LocationIpAddressSearchAsync(ipAddress.ToString());
            var currentConditions = await _weatherService.CurrentConditionsAsync(location.Key);

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