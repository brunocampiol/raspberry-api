using RaspberryPi.Application.Models.Dtos;

namespace RaspberryPi.Application.Interfaces
{
    public interface IWeatherAppService
    {
        Task<WeatherDto> GetWeatherFromIpAddress(string ipAddress);
        Task<WeatherDto> GetWeatherFromRandomIpAddressAsync();
    }
}