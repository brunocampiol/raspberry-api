using RaspberryPi.Domain.Core;

namespace RaspberryPi.Domain.Models.Entity
{
    public class AspNetUser : Core.EntityBase
    {
        public required string UserName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string Roles { get; set; }
        public DateTime DateCreateUTC { get; set; }
        public ICollection<Comment>? Posts { get; }
    }
}