using RaspberryPi.Application.Models.ViewModels;

namespace RaspberryPi.Application.Interfaces
{
    public interface IWeatherAppService
    {
        Task<WeatherViewModel> GetWeatherFromIpAddress(string ipAddress);
    }
}