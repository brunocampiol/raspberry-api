namespace RaspberryPi.Infrastructure.Models.Options
{
    public class EmailOptions
    {
        public const string SectionName = "EmailOptions";
        public required string FromEmail { get; init; }
        public required string APIKey { get; init; }
        public required string APISecret { get; init; }
        public required Uri BaseUrl { get; init; }
    }
}