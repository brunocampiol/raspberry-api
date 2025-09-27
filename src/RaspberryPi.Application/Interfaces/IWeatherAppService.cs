using RaspberryPi.Application.Models.Dtos;

namespace RaspberryPi.Application.Interfaces;

public interface IWeatherAppService
{
    Task<WeatherDto> CurrentWeatherFromIpAddress(string ipAddress);
    Task<WeatherDto> CurrentWeatherFromRandomIpAddressAsync();
    Task<WeatherDto> CurrentRandomWeatherFromInfraAsync();
    Task<WeatherDto> CurrentWeatherFromInfraAsync(double latitude, double longitude);
}