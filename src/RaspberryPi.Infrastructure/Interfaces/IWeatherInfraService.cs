using RaspberryPi.Infrastructure.Models.Weather;

namespace RaspberryPi.Infrastructure.Interfaces;

public interface IWeatherInfraService
{
    Task<WeatherInfraResponse> CurrentAsync(double latitude, double longitude);
}