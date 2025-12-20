using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Infrastructure.Models.Weather;

namespace RaspberryPi.Application.Interfaces;

public interface IWeatherAppService
{
    Task<WeatherInfraResponse> GetWeatherFromInfraAsync(double latitude, double longitude);
    Task<WeatherDto> CurrentWeatherFromIpAddressAsync(string ipAddress);
    Task<WeatherDto> CurrentWeatherFromRandomIpAddressAsync();
}