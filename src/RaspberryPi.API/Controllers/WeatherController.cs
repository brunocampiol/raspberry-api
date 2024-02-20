using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using System.Net;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// AccuWeather service related methods 
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherAppService _service;

        public WeatherController(IWeatherAppService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns weather data from given IP address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> FromIpAddress(string ipAddress)
        {            
            var result = await _service.GetWeatherFromIpAddress(ipAddress);
            return Ok(result);
        }

        /// <summary>
        /// Returns weather data from a random IP address
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> FromContextIpAddress()
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
            string forwardedHeader = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                clientIp = forwardedHeader.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(ip => ip.Trim())
                                          .FirstOrDefault(ip => !IPAddress.IsLoopback(IPAddress.Parse(ip)));
            }

            var result = await _service.GetWeatherFromIpAddress(clientIp);
            return Ok(result);
        }
    }
}