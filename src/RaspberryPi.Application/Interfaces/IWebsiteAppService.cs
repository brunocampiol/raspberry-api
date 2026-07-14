using RaspberryPi.Application.Models.Dtos;

namespace RaspberryPi.Application.Interfaces;

public interface IWebsiteAppService
{
    Task<string> GetRandomFactAsync();
    Task<WeatherDto> GetWeatherAsync(string ipAddress);
}