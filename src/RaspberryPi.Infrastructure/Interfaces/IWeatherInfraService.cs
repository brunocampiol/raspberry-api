using RaspberryPi.Infrastructure.Models.Weather;

namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IWeatherInfraService
    {
        Task<IEnumerable<PostalCodeSearchInfraDto>> PostalCodeSearch(string country, string postalCode);
        Task<IEnumerable<WeatherCurrentConditionsInfraDto>> CurrentConditionsAsync(string key);
        Task<WeatherLocationInfraDto> LocationIpAddressSearchAsync(string ipAddress);
    }
}