namespace RaspberryPi.Infrastructure.Models.Options
{
    public class FactOptions
    {
        public const string SectionName = "FactOptions";
        public required string APIKey { get; init; }
        public required Uri BaseUrl { get; init; }
    }
}