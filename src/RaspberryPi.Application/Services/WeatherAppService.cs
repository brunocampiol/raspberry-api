using Microsoft.Extensions.Logging;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Common;
using RaspberryPi.Infrastructure.Interfaces;

namespace RaspberryPi.Application.Services
{
    public sealed class WeatherAppService : IWeatherAppService
    {
        private readonly IWeatherService _accuWeatherService;
        private readonly IGeoLocationService _apiIpService;
        private readonly ILogger _logger;

        public WeatherAppService(IWeatherService accuWeatherService,
                                    IGeoLocationService apiIpService,
                                    ILogger<WeatherAppService> logger)
        {
            _accuWeatherService = accuWeatherService;
            _apiIpService = apiIpService;
            _logger = logger;
        }

        public async Task<WeatherDto> GetWeatherFromIpAddress(string ipAddress)
        {
            var geoPositioning = await _apiIpService.LookUpAsync(ipAddress);

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

            var accuWeatherLocations = await _accuWeatherService.PostalCodeSearch(geoPositioning.CountryCode, geoPositioning.PostalCode);
            if (accuWeatherLocations == null || !accuWeatherLocations.Any())
            {
                _logger.LogWarning("AccuWeather location is null or empty result");
                return WeatherDto.NotAvailable();
            }

            var location = accuWeatherLocations.First();
            var currentConditions = await _accuWeatherService.CurrentConditionsAsync(location.Key);

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
            var location = await _accuWeatherService.LocationIpAddressSearchAsync(ipAddress.ToString());
            var currentConditions = await _accuWeatherService.CurrentConditionsAsync(location.Key);

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