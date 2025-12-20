using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Application.Interfaces;

public interface IWebsiteAppService
{
    Task<FactInfraDto> FetchAndStoreUniqueFactAsync();
    Task<WeatherDto> GetWeatherAsync(string ipAddress);
}