namespace RaspberryPi.Application.Models.Dtos;

public record EmailDto
{
    public required string To { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
    public bool IsBodyHtml { get; init; }
}