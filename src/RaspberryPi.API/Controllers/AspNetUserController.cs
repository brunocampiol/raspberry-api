using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models.Data;
using RaspberryPi.API.Models.Requests;
using RaspberryPi.API.Repositories;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AspNetUserController : ControllerBase
    {
        private readonly IAspNetUserRepository _repository;

        public AspNetUserController(IAspNetUserRepository repository)
        {
                _repository = repository;
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
        public void Post([FromBody] NewAspNetUserRequest model)
        {
            var user = new AspNetUser()
            {
                Email = model.Email,
                Password = model.Password,
                Role = "user",
                DateCreateUTC = DateTime.UtcNow
            };

            _repository.Add(user);
        }
    }
}