using MediatR;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Mapping;
using RaspberryPi.Domain.Commands;
using RaspberryPi.Domain.Data.Repositories;
using RaspberryPi.Domain.Models;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AspNetUserController : ControllerBase
    {
        private readonly IAspNetUserRepository _repository;
        private readonly IRequestToDomainMapper _mapper;
        private readonly IMediator _mediator;

        public AspNetUserController(IAspNetUserRepository repository, IRequestToDomainMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public AspNetUser? Get(Guid id)
        {
            return _repository.GetNoTracking(id);
        }

        [HttpGet("list")]
        public IEnumerable<AspNetUser> List()
        {
            return _repository.ListNoTracking();
        }

        [HttpPost]
        public CreateAspNetUserResponse Post([FromBody] CreateAspNetUserRequest model)
        {
            //var user = _mapper.CreateAspNetUserRequestToAspNetUser(model);
            //user.Role = "user";
            //user.DateCreateUTC = DateTime.UtcNow;

            //var user = new AspNetUser
            //{
            //    Email = model.Email,
            //    Password = model.Password,
            //    Role = "user",
            //    DateCreateUTC = DateTime.UtcNow
            //};

            var request = new CreateAspNetUserRequest
            {
                Email = model.Email,
                Password = model.Password
            };

            var result = _mediator.Send(request);

            return result.Result;
   
        }
    }
}