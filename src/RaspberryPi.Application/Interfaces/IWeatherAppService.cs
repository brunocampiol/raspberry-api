using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Models;

namespace RaspberryPi.Application.Interfaces;

public interface IWeatherAppService
{
    Task<Weather> GetInfraWeatherAsync(double latitude, double longitude);
    Task<WeatherDto> GetWeatherWorkflowAsync(string ipAddress);
    Task<WeatherDto> GetCurrentWeatherFromRandomIpAddressAsync();
    Task<WeatherDto> GetCurrentWeatherAsync(string ipAddress);
}