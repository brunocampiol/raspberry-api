using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;

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
        public async Task<IActionResult> SaveFactAndComputeHashAsync()
        {
            var result = await _service.SaveFactAndComputeHashAsync();
            return Ok(result);
        }
    }
}