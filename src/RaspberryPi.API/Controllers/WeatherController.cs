using AutoMapper;
using MethodTimer;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Extensions;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Infrastructure.Models.Weather;

namespace RaspberryPi.API.Controllers;

/// <summary>
/// AccuWeather service related methods 
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherAppService _service;
    private readonly IMapper _mapper;

    public WeatherController(IWeatherAppService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns weather data from given IP address
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<WeatherViewModel> FromIpAddress(string ipAddress)
    {  
        // TODO review and remove this old methods
        var result = await _service.CurrentWeatherFromIpAddressAsync(ipAddress);
        var viewModel = _mapper.Map<WeatherViewModel>(result);
        return viewModel;
    }

    /// <summary>
    /// Returns weather data from user IP address
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<WeatherViewModel> FromContextIpAddress()
    {
        // TODO review and remove this old methods
        var clientIp = HttpContext.GetClientIpAddress();
        var result = await _service.CurrentWeatherFromIpAddressAsync(clientIp);
        var viewModel = _mapper.Map<WeatherViewModel>(result);
        return viewModel;
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

        return await _service.GetWeatherFromInfraAsync(latitude, longitude);
    }

    [Time]
    [HttpGet]
    public async Task<WeatherInfraResponse> CurrentRandom()
    {
        var random = new Random();
        var latitude = (random.NextDouble() * 180 - 90);   // -90 to 90
        var longitude = (random.NextDouble() * 360 - 180); // -180 to 180
        return await _service.GetWeatherFromInfraAsync(latitude, longitude);
    }
}