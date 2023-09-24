namespace RaspberryPi.API.Models.Requests
{
    public class CreateAspNetUserRequest
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}
