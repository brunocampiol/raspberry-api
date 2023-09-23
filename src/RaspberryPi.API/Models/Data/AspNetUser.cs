namespace RaspberryPi.API.Models.Data
{
    public class AspNetUser : BaseEntity
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string Role { get; init; } = default!;
        public DateTime DateCreateUTC { get; init; }
        public ICollection<Comment> Posts { get; }
    }
}