namespace RaspberryPi.API.Models.Data
{
    public class Comment : BaseEntity
    {
        public string Text { get; init; } = default!;
        public DateTime DateCreated { get; init; } = DateTime.UtcNow;
    }
}