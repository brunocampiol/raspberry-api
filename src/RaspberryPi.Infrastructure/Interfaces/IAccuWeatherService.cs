using RaspberryPi.Infrastructure.Models.AccuWeather;

namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IAccuWeatherService
    {
        Task<IEnumerable<AccuWeatherCurrentConditionsResponse>> CurrentConditionsAsync(string key);
        Task<AccuWeatherLocationResponse> LocationIpAddressSearchAsync(string ipAddress);
    }
}