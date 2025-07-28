using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Models.Dtos;

public record DbBackupDto
{
    public required IEnumerable<Fact> Facts { get; set; }
    public required IEnumerable<GeoLocation> GeoLocations { get; set; }
    public required IEnumerable<FeedbackMessage> FeedbackMessages { get; set; }
    public required IEnumerable<EmailOutbox> EmailsOutbox { get; set; }
}