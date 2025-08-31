namespace RaspberryPi.Infrastructure.Models.Options;

public record EmailOptions
{
    public const string SectionName = "EmailOptions";
    public required string FromName { get; init; }
    public required string FromEmail { get; init; }
    public required string SmtpAddress { get; init; }
    public required string SmtpPassword { get; init; }
    public required int SmtpPort { get; init; }
}