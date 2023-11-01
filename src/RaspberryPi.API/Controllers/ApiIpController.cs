using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ApiIpController : ControllerBase
    {
        private readonly IApiIpService _service;

        public ApiIpController(IApiIpService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> LookUp([FromQuery][Required] string ipAddress)
        {
            var result = await _service.Check(ipAddress);
            return Ok(result);
        }
    }
}