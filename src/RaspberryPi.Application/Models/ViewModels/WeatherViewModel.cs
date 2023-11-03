namespace RaspberryPi.Application.Models.ViewModels
{
    public class WeatherViewModel
    {
        public string EnglishName { get; init; } = default!;
        public string CountyCode { get; init; } = default!;
        public string WeatherText { get; init; } = default!;
        public string Temperature { get; init; } = default!;

        public static WeatherViewModel NotAvailable()
        {
            return new WeatherViewModel
            {
                EnglishName = "N/A",
                CountyCode = "N/A",
                WeatherText = "N/A",
                Temperature = "N/A"
            };
        }
    }
}