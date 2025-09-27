using AutoMapper;
using MethodTimer;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Extensions;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Interfaces;

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
        var result = await _service.CurrentWeatherFromIpAddress(ipAddress);
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
        var clientIp = HttpContext.GetClientIpAddress();
        var result = await _service.CurrentWeatherFromIpAddress(clientIp);
        var viewModel = _mapper.Map<WeatherViewModel>(result);
        return viewModel;
    }

    /// <summary>
    /// Returns a random place weather data
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<WeatherViewModel> CurrentRandomWeatherFromInfra()
    {
        var result = await _service.CurrentRandomWeatherFromInfraAsync();
        var viewModel = _mapper.Map<WeatherViewModel>(result);
        return viewModel;
    }

    /// <summary>
    /// Returns weather data from longitude and latitude coordinates
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<WeatherViewModel> CurrentWeatherFromInfra(float latitude, float longitude)
    {
        var result = await _service.CurrentWeatherFromInfraAsync(latitude, longitude);
        var viewModel = _mapper.Map<WeatherViewModel>(result);
        return viewModel;
    }
}