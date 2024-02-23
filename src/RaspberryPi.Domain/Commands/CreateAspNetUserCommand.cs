using FluentValidation.Results;
using MediatR;
using RaspberryPi.Domain.Core;

namespace RaspberryPi.Domain.Commands
{
    public class CreateAspNetUserCommand : Command, IRequest<ValidationResult>
    {
        public required string UserName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}