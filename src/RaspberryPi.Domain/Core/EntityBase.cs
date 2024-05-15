namespace RaspberryPi.Domain.Core
{
    public abstract class EntityBase : IEntityBase
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}