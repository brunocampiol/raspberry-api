namespace RaspberryPi.Domain.Models
{
    public class AspNetUser
    {
        public required string UserName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string Roles { get; set; }
    }
}