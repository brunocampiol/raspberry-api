namespace RaspberryPi.Application.Models.ViewModels
{
    public class RegisterAspNetUserViewModel
    {
        public required string UserName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}