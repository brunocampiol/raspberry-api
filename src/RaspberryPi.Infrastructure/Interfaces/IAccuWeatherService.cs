using RaspberryPi.Infrastructure.Models.AccuWeather;

namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IAccuWeatherService
    {
        Task<IEnumerable<PostalCodeSearch>> PostalCodeSearch(string country, string postalCode);
        Task<IEnumerable<AccuWeatherCurrentConditionsResponse>> CurrentConditionsAsync(string key);
        Task<AccuWeatherLocationResponse> LocationIpAddressSearchAsync(string ipAddress);
    }
}