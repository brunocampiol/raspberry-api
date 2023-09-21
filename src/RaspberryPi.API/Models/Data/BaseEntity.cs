namespace RaspberryPi.API.Models.Data
{
    public abstract class BaseEntity
    {
        public string Pk => Id.ToString();
        public string Sk => Pk;

        public Guid Id { get; init; } = Guid.NewGuid();
    }
}
