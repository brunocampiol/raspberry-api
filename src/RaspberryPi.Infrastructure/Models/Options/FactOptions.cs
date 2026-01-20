namespace RaspberryPi.Infrastructure.Models.Options;

public record FactOptions
{
    public const string SectionName = "FactOptions";
    public required Uri BaseUrl { get; init; }
}