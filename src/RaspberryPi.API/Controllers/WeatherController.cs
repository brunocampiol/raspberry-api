using Fetchgoods.Text.Json.Extensions;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using System.Net;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger _logger; // TODO remove logger
        private readonly IWeatherAppService _service;

        public WeatherController(ILogger<WeatherController> logger,
                                 IWeatherAppService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> FromIpAddress(string ipAddress)
        {            
            var result = await _service.GetWeatherFromIpAddress(ipAddress);
            
            var logMessage = $"Location for '{ipAddress}' resulted in: '{result.ToJson()}'";
            _logger.LogInformation(logMessage);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> FromContextIpAddress()
        {
            // TODO move context logic to service
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