namespace RaspberryPi.Infrastructure.Models.Options
{
    public class WeatherOptions
    {
        public const string SectionName = "WeatherOptions";
        public string ApiKey { get; init; } = default!;
        public Uri BaseUrl { get; set; } = default!;
    }
}