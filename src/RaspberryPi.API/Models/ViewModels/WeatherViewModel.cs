namespace RaspberryPi.API.Models.ViewModels;

public record WeatherViewModel
{
    /// <summary>
    /// The location name in English.
    /// </summary>
    public required string EnglishName { get; init; } // TODO: change to location name

    /// <summary>
    /// The two-letter country code (ISO 3166-1).
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    /// A weather description in English.
    /// </summary>
    public required string WeatherText { get; init; }

    /// <summary>
    /// Temperature in degrees Celsius.
    /// </summary>
    public required string Temperature { get; init; }
}