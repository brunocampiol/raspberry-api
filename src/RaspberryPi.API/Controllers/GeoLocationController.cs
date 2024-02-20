using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// API IP service related methods
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class GeoLocationController : ControllerBase
    {
        private readonly IGeoLocationAppService _appService;

        public GeoLocationController(IGeoLocationAppService appService)
        {
            _appService = appService;
        }

        /// <summary>
        /// Returns geolocation data based on given IP address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> LookUp([FromQuery][Required] string ipAddress)
        {
            var result = await _appService.LookUpAsync(ipAddress);
            return Ok(result);
        }

        /// <summary>
        /// Returns geolocation data for a random IP address
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> LookUpFromRandomIpAddress()
        {
            var result = await _appService.LookUpFromRandomIpAddressAsync();
            return Ok(result);
        }
    }
}