using Fetchgoods.Text.Json.Extensions;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWeatherAppService _service;

        public WeatherController(ILogger<WeatherController> logger,
                                 IWeatherAppService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> FromIpAddress()
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await _service.GetWeatherFromIpAddress(remoteIpAddress);
            
            var logMessage = $"Location for '{remoteIpAddress}' resulted in: '{result.ToJson()}'";
            _logger.LogInformation(logMessage);

            return Ok(result);
        }
    }
}