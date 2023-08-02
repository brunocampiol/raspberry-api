namespace RaspberryPi.API.Contracts.Data
{
    public class Comment
    {
        // json prop name required ???
        public string Pk => Id;
        public string Sk => Pk;

        public string Id { get; init; } = default!;
        public string Text { get; init; } = default!;
        public DateTime DateCreated { get; init; } = DateTime.UtcNow;
    }
}