namespace RaspberryPi.Infrastructure.Models.Options
{
    public class ApiIpOptions
    {
        public const string SectionName = "ApiIpOptions";
        public string APIKey { get; init; } = default!;
        public Uri BaseUrl { get; set; } = default!;
    }
}