using RaspberryPi.Domain.Core;

namespace RaspberryPi.Domain.Models.Entity;

// TODO what about change everything to GeoLocationEntity / GeoLocationData / GeoLocationRecord

public class GeoLocation : EntityBase
{
    public required string CountryCode { get; init; }
    public string? PostalCode { get; init; }
    public required string LocationName { get; init; }
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
    public required DateTime CreatedAtUTC { get; init; }
}