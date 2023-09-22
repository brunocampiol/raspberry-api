namespace RaspberryPi.API.Models.Data
{
    public abstract class BaseEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}