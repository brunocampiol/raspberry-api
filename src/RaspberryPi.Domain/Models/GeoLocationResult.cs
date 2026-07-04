namespace RaspberryPi.Domain.Models;

/// <summary>
/// Standardised geo location response from any provider.
/// </summary>
public class GeoLocationResult
{
    public string? PostalCode { get; init; }
    public required string Provider { get; init; }
    public required string CountryCode { get; init; }
    public required string LocationName { get; init; }
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
}