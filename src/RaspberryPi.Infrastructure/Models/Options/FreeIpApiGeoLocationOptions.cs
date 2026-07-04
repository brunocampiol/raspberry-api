namespace RaspberryPi.Infrastructure.Models.Options;

public record FreeIpApiGeoLocationOptions
{
    public const string SectionName = "FreeIpApiGeoLocationOptions";
    public required string APIKey { get; init; }
    public required Uri BaseUrl { get; set; }
}