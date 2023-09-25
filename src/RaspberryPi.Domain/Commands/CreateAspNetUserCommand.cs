using FluentValidation.Results;
using MediatR;
using RaspberryPi.Domain.Core;

namespace RaspberryPi.Domain.Commands
{
    public class CreateAspNetUserCommand : Command, IRequest<ValidationResult>
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}