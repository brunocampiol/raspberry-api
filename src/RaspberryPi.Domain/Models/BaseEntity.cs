namespace RaspberryPi.Domain.Models
{
    public abstract class BaseEntity : IEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}
