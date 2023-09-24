using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Mapping;
using RaspberryPi.API.Models.Requests;
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

        public AspNetUserController(IAspNetUserRepository repository, IRequestToDomainMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
        public void Post([FromBody] CreateAspNetUserRequest model)
        {
            //var user = _mapper.CreateAspNetUserRequestToAspNetUser(model);
            //user.Role = "user";
            //user.DateCreateUTC = DateTime.UtcNow;

            var user = new AspNetUser
            {
                Email = model.Email,
                Password = model.Password,
                Role = "user",
                DateCreateUTC = DateTime.UtcNow
            };

            _repository.Add(user);
            _repository.UnitOfWork.Commit();
        }
    }
}