namespace RaspberryPi.API.Models;

public record RequestDetail
{
    public required string IpAddress { get; init; }
    public required DateTime TimestampUtc { get; init; }
}