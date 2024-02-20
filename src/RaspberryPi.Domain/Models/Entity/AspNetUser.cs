namespace RaspberryPi.Domain.Models.Entity
{
    public class AspNetUser : BaseEntity
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string Role { get; set; } = default!;
        public DateTime DateCreateUTC { get; set; }
        public ICollection<Comment> Posts { get; }
    }
}
