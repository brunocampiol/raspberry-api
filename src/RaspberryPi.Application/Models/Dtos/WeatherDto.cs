using RaspberryPi.Application.Models.ViewModels;

namespace RaspberryPi.Application.Models.Dtos
{
    public class WeatherDto
    {
        public string EnglishName { get; init; } = default!;
        public string CountryCode { get; init; } = default!;
        public string WeatherText { get; init; } = default!;
        public string Temperature { get; init; } = default!;

        public static WeatherDto NotAvailable()
        {
            return new WeatherDto
            {
                EnglishName = "N/A",
                CountryCode = "N/A",
                WeatherText = "N/A",
                Temperature = "N/A"
            };
        }
    }
}