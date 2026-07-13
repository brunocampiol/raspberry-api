using RaspberryPi.Domain.Models;

namespace RaspberryPi.Infrastructure.Interfaces;

public interface IWeatherInfraService
{
    Task<Weather> CurrentAsync(double latitude, double longitude);
}