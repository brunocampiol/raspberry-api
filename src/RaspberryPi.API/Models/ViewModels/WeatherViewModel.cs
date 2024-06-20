namespace RaspberryPi.API.Models.ViewModels
{
    public class WeatherViewModel
    {
        public string EnglishName { get; init; } = default!;
        public string CountryCode { get; init; } = default!;
        public string WeatherText { get; init; } = default!;
        public string Temperature { get; init; } = default!;
    }
}