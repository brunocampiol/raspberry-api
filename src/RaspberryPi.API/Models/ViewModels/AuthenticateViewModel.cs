namespace RaspberryPi.API.Models.ViewModels
{
    public class AuthenticateViewModel
    {
        public required string UserName { get; init; }
        public required string Password { get; init; }
    }
}