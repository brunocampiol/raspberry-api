namespace RaspberryPi.Domain.Core
{
    public interface IAggregateRoot
    {
        Guid Id { get; init; }
    }
}