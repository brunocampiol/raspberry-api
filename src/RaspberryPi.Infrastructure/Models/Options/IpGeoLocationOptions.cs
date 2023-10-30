namespace RaspberryPi.Infrastructure.Models.Options
{
    public class IpGeoLocationOptions
    {
        public const string SectionName = "IpGeoLocationOptions";
        public string APIKey { get; init; } = default!;
        public Uri BaseUrl { get; set; } = default!;
    }
}