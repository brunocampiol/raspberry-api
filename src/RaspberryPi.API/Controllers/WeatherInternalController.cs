using MethodTimer;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Weather;

namespace RaspberryPi.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherInternalController : ControllerBase
{
    private readonly IWeatherInfraService _weatherInfraService;

    public WeatherInternalController(IWeatherInfraService weatherInfraService)
    {
        _weatherInfraService = weatherInfraService ?? throw new ArgumentNullException(nameof(weatherInfraService));
    }

    [Time]
    [HttpGet]
    public async Task<WeatherInfraResponse> Current(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
        {
            throw new BadHttpRequestException("Latitude must be between -90 and 90 degrees");
        }

        if (longitude < -180 || longitude > 180)
        {
            throw new BadHttpRequestException("Longitude must be between -180 and 180 degrees");
        }

        return await _weatherInfraService.CurrentAsync(latitude, longitude);
    }

    [Time]
    [HttpGet]
    public async Task<WeatherInfraResponse> CurrentRandom()
    {
        var random = new Random();
        var latitude = (random.NextDouble() * 180 - 90);   // -90 to 90
        var longitude = (random.NextDouble() * 360 - 180); // -180 to 180
        return await _weatherInfraService.CurrentAsync(latitude, longitude);
    }
}