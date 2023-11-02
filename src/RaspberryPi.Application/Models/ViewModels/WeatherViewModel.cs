namespace RaspberryPi.Application.Models.ViewModels
{
    public class WeatherViewModel
    {
        public string EnglishName { get; init; }
        public string WeatherText { get; init; }
        public string Temperature { get; init; }

        public static WeatherViewModel NotAvailable()
        {
            return new WeatherViewModel
            {
                EnglishName = "N/A",
                WeatherText = "N/A",
                Temperature = "N/A"
            };
        }
    }
}