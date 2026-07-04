namespace RaspberryPi.Infrastructure.Models.Options;

public record IpGeoLocationOptions
{
    public const string SectionName = "IpGeoLocationOptions";
    public required string APIKey { get; init; }
    public required Uri BaseUrl { get; set; }
}