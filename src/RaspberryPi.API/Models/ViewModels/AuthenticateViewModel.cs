namespace RaspberryPi.API.Models.ViewModels;

public record AuthenticateViewModel
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
}