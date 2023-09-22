using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models.Data;
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
            return _repository.Get(id);
        }

        [HttpPost]
        public void Post([FromBody] AspNetUser model)
        {
            _repository.Add(model);
        }
    }
}