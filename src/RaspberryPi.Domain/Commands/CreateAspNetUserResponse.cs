namespace RaspberryPi.Domain.Commands
{
    public class CreateAspNetUserResponse
    {
        public string? Message { get; set; }
        public bool IsSuccess { get; init; }
    }
}