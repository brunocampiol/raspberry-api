using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.ViewModels;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.AccuWeather;

namespace RaspberryPi.Application.Services
{
    public class WeatherAppService : IWeatherAppService
    {
        private readonly IAccuWeatherService _accuWeatherService;

        public WeatherAppService(IAccuWeatherService accuWeatherService)
        {
            _accuWeatherService = accuWeatherService;
        }

        public async Task<WeatherViewModel> GetWeatherFromIpAddress(string ipAddress)
        {
            var locationResult = await _accuWeatherService.LocationIpAddressSearchAsync(ipAddress);
            IEnumerable<AccuWeatherCurrentConditionsResponse> currentCondition;
            string cityName = locationResult.EnglishName;

            if (locationResult.ParentCity != null)
            {
                currentCondition = await _accuWeatherService.CurrentConditionsAsync(locationResult.ParentCity.Key);
                cityName = locationResult.ParentCity.EnglishName;
            }
            else
            {
                currentCondition = await _accuWeatherService.CurrentConditionsAsync(locationResult.Key);
            }

            var viewModel = new WeatherViewModel
            {
                EnglishName = cityName,
                WeatherText = currentCondition.First().WeatherText,
                Temperature = currentCondition.First().Temperature.Metric.Value + "°C"
            };

            return viewModel;
        }
    }
}
