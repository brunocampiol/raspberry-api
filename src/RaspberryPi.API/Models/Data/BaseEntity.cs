namespace RaspberryPi.API.Models.Data
{
    public abstract class BaseEntity : IEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}