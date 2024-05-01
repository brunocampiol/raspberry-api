using RaspberryPi.Domain.Core;

namespace RaspberryPi.Domain.Models.Entity
{
    public class Fact : EntityBase
    {
        public required string Text { get; init; }
        public required DateTime CreatedAt { get; init; }
    }
}