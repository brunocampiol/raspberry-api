using RaspberryPi.Domain.Core;

namespace RaspberryPi.Domain.Models.Entity
{
    public class FeedbackMessage : EntityBase
    {
        public required string Message { get; init; }
        public string? CountryCode { get; init; }
        public string? PostalCode { get; init; }
        public string? City { get; init; }
        public string? RegionName { get; init; }
        public string? HttpHeaders { get; init; }
        public required DateTime CreatedAtUTC { get; init; }
    }
}