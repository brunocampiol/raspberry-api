using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ApiIpController : ControllerBase
    {
        private readonly IApiIPService _service;

        public ApiIpController(IApiIPService service)
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