using RaspberryPi.Domain.Core;

namespace RaspberryPi.Domain.Models.Entity;

public class GeoLocation : EntityBase
{
    public required string CountryCode { get; init; }
    public required string PostalCode { get; init; }
    public required string City { get; init; }
    public required string RegionName { get; init; }
    public required string WeatherKey { get; init; }
    public required DateTime CreatedAtUTC { get; init; }
}