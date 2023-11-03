using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GeoLocationController : ControllerBase
    {
        private readonly IGeoLocationService _service;

        public GeoLocationController(IGeoLocationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> LookUp([FromQuery][Required] string ipAddress)
        {
            var result = await _service.LookUp(ipAddress);
            return Ok(result);
        }
    }
}