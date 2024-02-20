using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net;

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

        [HttpGet]
        public async Task<IActionResult> LookUpFromRandomIpAddress()
        {
            // TODO move to app service layer
            var random = new Random();
            byte[] ipAddressBytes = new byte[4];
            random.NextBytes(ipAddressBytes);
            var ipAddress = new IPAddress(ipAddressBytes);

            var result = await _service.LookUp(ipAddress.ToString());
            return Ok(result);
        }
    }
}