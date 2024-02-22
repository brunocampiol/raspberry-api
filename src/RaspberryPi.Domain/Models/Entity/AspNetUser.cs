namespace RaspberryPi.Domain.Models.Entity
{
    public class AspNetUser : BaseEntity
    {
        public string UserName { get; init; } = default!; // TODO add this prop to migration db
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string Role { get; set; } = default!;
        public DateTime DateCreateUTC { get; set; }
        public ICollection<Comment> Posts { get; }
    }
}