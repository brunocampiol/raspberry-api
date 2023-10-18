using Fetchgoods.Text.Json.Extensions;
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

        [HttpGet]
        public async Task<IActionResult> FromIpAddress()
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            remoteIpAddress = "189.6.243.110";
            var locationResult = await _service.LocationIpAddressSearchAsync(remoteIpAddress);
            var weather = await _service.CurrentConditionsAsync(locationResult.Key);


            //var logMessage = $"Location for '{remoteIpAddress}' resulted in: '{locationResult.ToJson()}'";
            //_logger.LogInformation(logMessage);

            return Ok(weather);
        }
    }
}