using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Infrastructure.Interfaces;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAccuWeatherService _service;

        public WeatherController(ILogger<WeatherController> logger,
                                 IAccuWeatherService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> FromIpAddress()
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await _service.LocationIpAddressSearchAsync(remoteIpAddress);

            var logMessage = $"Location for '{remoteIpAddress}' resulted in: '{result}'";
            _logger.LogInformation(logMessage);

            return Ok(result);
        }
    }
}