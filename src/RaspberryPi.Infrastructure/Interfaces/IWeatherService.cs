using RaspberryPi.Infrastructure.Models.Weather;

namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IWeatherService
    {
        Task<IEnumerable<PostalCodeSearchResponse>> PostalCodeSearch(string country, string postalCode);
        Task<IEnumerable<WeatherCurrentConditionsResponse>> CurrentConditionsAsync(string key);
        Task<WeatherLocationResponse> LocationIpAddressSearchAsync(string ipAddress);
    }
}