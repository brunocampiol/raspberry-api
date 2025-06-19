namespace RaspberryPi.Infrastructure.Models.Options;

public record WeatherOptions
{
    public const string SectionName = "WeatherOptions";
    public required string ApiKey { get; init; }
    public required Uri BaseUrl { get; set; }
}