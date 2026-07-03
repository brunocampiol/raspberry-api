namespace RaspberryPi.Infrastructure.Models.Options;

public record Ip2LocationGeoLocationOptions
{
    public const string SectionName = "Ip2LocationGeoLocationOptions";
    public required string APIKey { get; init; }
    public required Uri BaseUrl { get; set; }
}