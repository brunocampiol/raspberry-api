namespace RaspberryPi.Application.Models.Dtos;

public record WeatherDto
{
    public required string EnglishName { get; init; }
    public required string CountryCode { get; init; }
    public required string WeatherText { get; init; }
    public required string Temperature { get; init; }

    public static WeatherDto NotAvailable()
    {
        return new WeatherDto
        {
            EnglishName = "N/A",
            CountryCode = "N/A",
            WeatherText = "N/A",
            Temperature = "N/A"
        };
    }
}