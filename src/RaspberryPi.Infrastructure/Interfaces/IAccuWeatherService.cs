namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IAccuWeatherService
    {
        Task<string> CurrentConditions(int key);
        Task<string> LocationIpAddressSearchAsync(string ipAddress);
    }
}