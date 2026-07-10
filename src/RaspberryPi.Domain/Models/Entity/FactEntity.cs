using RaspberryPi.Domain.Core;

namespace RaspberryPi.Domain.Models.Entity;

public class FactEntity : EntityBase
{
    public required string Text { get; init; }
    public required string TextHash { get; init; }
    public required DateTime CreatedAt { get; init; }
}