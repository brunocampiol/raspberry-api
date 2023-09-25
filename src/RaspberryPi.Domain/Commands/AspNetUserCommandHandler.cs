using FluentValidation.Results;
using MediatR;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Interfaces;
using RaspberryPi.Domain.Models;

namespace RaspberryPi.Domain.Commands
{
    public sealed class AspNetUserCommandHandler : CommandHandler,
        IRequestHandler<CreateAspNetUserCommand, ValidationResult>
    {
        private readonly IAspNetUserRepository _repository;

        public AspNetUserCommandHandler(IAspNetUserRepository repository)
        {
            _repository = repository;
        }

        public Task<ValidationResult> Handle(CreateAspNetUserCommand request, CancellationToken cancellationToken)
        {
            request.IsValid();

            // Verifica se o cliente ja existe
            // Valida dados
            // Insere o cliente
            var user = new AspNetUser
            {
                Email = request.Email,
                Password = request.Password,
                Role = "user",
                DateCreateUTC = DateTime.UtcNow,
            };
            _repository.Add(user);
            //_repository.UnitOfWork.Commit();
            Commit(_repository.UnitOfWork);

            // Envia email de boas vindas
            var result = new ValidationResult();
            return Task.FromResult(result);
        }
    }
}