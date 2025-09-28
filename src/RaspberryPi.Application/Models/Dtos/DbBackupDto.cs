using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Models.Dtos;

public record DbBackupDto
{
    public IEnumerable<Fact> Facts { get; init; } = [];
    public IEnumerable<GeoLocation> GeoLocations { get; init; } = [];
    public IEnumerable<FeedbackMessage> FeedbackMessages { get; init; } = [];
    public IEnumerable<EmailOutbox> EmailsOutbox { get; init; } = [];
}