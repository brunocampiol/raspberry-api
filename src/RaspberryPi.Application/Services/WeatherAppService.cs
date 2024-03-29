﻿using Microsoft.Extensions.Logging;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.ViewModels;
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

        public async Task<WeatherViewModel> GetWeatherFromIpAddress(string ipAddress)
        {
            var geoPositioning = await _apiIpService.LookUpAsync(ipAddress);

            if (string.IsNullOrEmpty(geoPositioning.CountryCode))
            {
                _logger.LogWarning("GeoPositioning CountryCode is null, empty or white-space characters");
                return WeatherViewModel.NotAvailable();
            }
            if (string.IsNullOrWhiteSpace(geoPositioning.PostalCode))
            {
                _logger.LogWarning("GeoPositioning PostalCode is null, empty or white-space characters");
                return WeatherViewModel.NotAvailable();
            }
            if (string.IsNullOrWhiteSpace(geoPositioning.City))
            {
                _logger.LogWarning("GeoPositioning City is null, empty or white-space characters");
                return WeatherViewModel.NotAvailable();
            }

            var accuWeatherLocations = await _accuWeatherService.PostalCodeSearch(geoPositioning.CountryCode, geoPositioning.PostalCode);
            if (accuWeatherLocations == null || !accuWeatherLocations.Any())
            {
                _logger.LogWarning("AccuWeather location is null or empty result");
                return WeatherViewModel.NotAvailable();
            }

            var location = accuWeatherLocations.First();
            var currentConditions = await _accuWeatherService.CurrentConditionsAsync(location.Key);

            var viewModel = new WeatherViewModel
            {
                EnglishName = geoPositioning.City,
                CountryCode = geoPositioning.CountryCode,
                WeatherText = currentConditions.First().WeatherText,
                Temperature = currentConditions.First().Temperature.Metric.Value + "°C"
            };

            return viewModel;
        }
    }
}