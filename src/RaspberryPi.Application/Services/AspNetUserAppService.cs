using FluentValidation.Results;
using RaspberryPi.Application.Models.ViewModels;
using RaspberryPi.Domain.Commands;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Services
{
    public sealed class AspNetUserAppService : IAspNetUserAppService
    {
        private readonly IMediatorHandler _mediator;
        private readonly IAspNetUserRepository _repository;

        public AspNetUserAppService(IMediatorHandler mediator, IAspNetUserRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public AspNetUser? Get(Guid id)
        {
            return _repository.GetNoTracking(id);
        }

        public IEnumerable<AspNetUser> List()
        {
            return _repository.ListNoTracking();
        }

        public ValidationResult Register(RegisterAspNetUserViewModel viewModel)
        {
            var command = new CreateAspNetUserCommand
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                Password = viewModel.Password,
            };

            var result = _mediator.SendCommand(command);
            return result.Result;
        }
    }
}