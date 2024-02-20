using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GeoLocationController : ControllerBase
    {
        private readonly IGeoLocationAppService _appService;

        public GeoLocationController(IGeoLocationAppService appService)
        {
            _appService = appService;
        }

        [HttpGet]
        public async Task<IActionResult> LookUp([FromQuery][Required] string ipAddress)
        {
            var result = await _appService.LookUpAsync(ipAddress);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> LookUpFromRandomIpAddress()
        {
            var result = await _appService.LookUpFromRandomIpAddressAsync();
            return Ok(result);
        }
    }
}