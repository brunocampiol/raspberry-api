using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Models.Dtos;

public record DbBackupDto
{
    public IEnumerable<FactEntity> Facts { get; init; } = [];
    public IEnumerable<GeoLocationEntity> GeoLocations { get; init; } = [];
    public IEnumerable<FeedbackMessageEntity> FeedbackMessages { get; init; } = [];
    public IEnumerable<EmailOutboxEntity> EmailsOutbox { get; init; } = [];
}