using MediatR;

namespace RaspberryPi.Domain.Commands
{
    public class CreateAspNetUserRequest : IRequest<CreateAspNetUserResponse>
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}