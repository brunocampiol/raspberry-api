using MediatR;
using RaspberryPi.Domain.Data.Repositories;
using RaspberryPi.Domain.Models;

namespace RaspberryPi.Domain.Commands
{
    public sealed class CreateAspNetUserHandler : IRequestHandler<CreateAspNetUserRequest, CreateAspNetUserResponse>
    {
        private readonly IAspNetUserRepository _repository;

        public CreateAspNetUserHandler(IAspNetUserRepository repository)
        {
            _repository = repository;
        }

        public Task<CreateAspNetUserResponse> Handle(CreateAspNetUserRequest request, CancellationToken cancellationToken)
        {
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
            _repository.UnitOfWork.Commit();
            // Envia email de boas vindas
            var result = new CreateAspNetUserResponse
            {
                IsSuccess = true,
                Message = $"user '{user.Email}' created"
            };

            return Task.FromResult(result);
        }
    }
}