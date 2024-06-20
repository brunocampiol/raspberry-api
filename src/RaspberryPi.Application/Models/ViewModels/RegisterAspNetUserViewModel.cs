namespace RaspberryPi.Application.Models.ViewModels
{
    // TODO: how to move this to the API level?
    public class RegisterAspNetUserViewModel
    {
        public required string UserName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}