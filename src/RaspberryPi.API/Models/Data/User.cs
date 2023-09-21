namespace RaspberryPi.API.Models.Data
{
    public class User
    {
        public Guid Id { get; init; }
        public string Username { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string Role { get; init; } = default!;
        public DateTime DateCreateUTC { get; init; }
    }
}