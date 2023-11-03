namespace RaspberryPi.Infrastructure.Models.Options
{
    public class GeoLocationOptions
    {
        public const string SectionName = "GeoLocationOptions";
        public string APIKey { get; init; } = default!;
        public Uri BaseUrl { get; set; } = default!;
    }
}