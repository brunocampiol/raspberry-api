namespace RaspberryPi.API.Models.Requests
{
    public class TokenGenerationRequest
    {
        public string UserName { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}