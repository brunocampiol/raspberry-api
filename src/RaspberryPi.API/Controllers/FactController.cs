using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FactController : ControllerBase
    {
        private readonly IFactAppService _service;

        public FactController(IFactAppService service)
        {
            _service = service;   
        }

        [HttpGet]
        public async Task<IEnumerable<Fact>> GetAllDatabaseFacts()
        {
            var result = await _service.GetAllDatabaseFactsAsync();
            return result;
        }

        [HttpGet]
        public async Task<long> CountAllDatabaseFacts()
        {
            var result = await _service.CountAllDatabaseFacts();
            return result;
        }

        [HttpGet]
        public async Task<FactResponse> SaveFactAndComputeHash()
        {
            var result = await _service.SaveFactAndComputeHashAsync();
            return result;
        }
    }
}