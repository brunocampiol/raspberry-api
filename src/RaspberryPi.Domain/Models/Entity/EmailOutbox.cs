using RaspberryPi.Domain.Core;

namespace RaspberryPi.Domain.Models.Entity
{
    public class EmailOutbox : EntityBase
    {
        public required string From { get; init; }
        public required string To { get; init; }
        public required string Subject { get; init; }
        public required string Body { get; init; }
        public required DateTime SentAtUTC { get; init; }
    }
}