namespace RaspberryPi.Infrastructure.Models.Options;

public record GeoLocationOptions
{
    public const string SectionName = "GeoLocationOptions";
    public  required string APIKey { get; init; }
    public required Uri BaseUrl { get; set; }
}