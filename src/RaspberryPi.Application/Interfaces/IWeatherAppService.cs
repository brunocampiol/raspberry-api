using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Infrastructure.Models.Weather;

namespace RaspberryPi.Application.Interfaces;

public interface IWeatherAppService
{
    Task<WeatherInfraResponse> GetInfraWeatherAsync(double latitude, double longitude);
    Task<WeatherDto> GetWeatherWorkflowAsync(string ipAddress);
    Task<WeatherDto> GetCurrentWeatherFromRandomIpAddressAsync();
    Task<WeatherDto> GetCurrentWeatherAsync(string ipAddress);
}