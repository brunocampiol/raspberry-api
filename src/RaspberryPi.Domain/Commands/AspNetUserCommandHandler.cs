using FluentValidation.Results;
using MediatR;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;

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
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                Roles = "user",
                DateCreateUTC = DateTime.UtcNow,
            };
            //await _repository.AddAsync(user);
            //_repository.UnitOfWork.Commit();
            //Commit(_repository.UnitOfWork);

            // Envia email de boas vindas
            var result = new ValidationResult();
            return Task.FromResult(result);
        }
    }
}