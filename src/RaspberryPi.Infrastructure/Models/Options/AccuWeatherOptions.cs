namespace RaspberryPi.Infrastructure.Models.Options
{
    public class AccuWeatherOptions
    {
        public const string SectionName = "AccuWeatherOptions";
        public string ApiKey { get; init; } = default!;
        public Uri BaseUrl { get; set; } = default!;
    }
}