namespace RaspberryPi.API.Models.ViewModels;

public record WeatherViewModel
{
    /// <summary>
    /// The location name in English.
    /// </summary>
    public required string LocationName { get; init; }

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

    // TODO Temperature is a string, it might be worth renaming it to TemperatureDisplay
    // or TemperatureFormatted if it includes the °C symbol, just to differentiate it
    // from a raw numeric decimal or int value!)
}