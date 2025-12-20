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
    /// Returns weather data from given IP address.
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<WeatherViewModel> FromIpAddress(string ipAddress)
    {  
        var result = await _service.GetCurrentWeatherAsync(ipAddress);
        var viewModel = _mapper.Map<WeatherViewModel>(result);
        return viewModel;
    }

    /// <summary>
    /// Returns weather data from user IP address.
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<WeatherViewModel> FromContextIpAddress()
    {
        var clientIp = HttpContext.GetClientIpAddress();
        var result = await _service.GetCurrentWeatherAsync(clientIp);
        var viewModel = _mapper.Map<WeatherViewModel>(result);
        return viewModel;
    }

    /// <summary>
    /// Returns weather data from random IP address.
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<WeatherViewModel> FromRandomIpAddress()
    {
        var result = await _service.GetCurrentWeatherFromRandomIpAddressAsync();
        var viewModel = _mapper.Map<WeatherViewModel>(result);
        return viewModel;
    }

    /// <summary>
    /// Gets weather infra model by latitude and longitude.
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <returns></returns>
    /// <exception cref="BadHttpRequestException"></exception>
    [Time]
    [HttpGet]
    public async Task<WeatherInfraResponse> Current(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
        {
            var errorMessage = $"Latitude must be between -90 and 90 degrees and not '{latitude}'";
            throw new BadHttpRequestException(errorMessage);
        }

        if (longitude < -180 || longitude > 180)
        {
            var errorMessage = $"Longitude must be between -180 and 180 degrees and not '{longitude}'";
            throw new BadHttpRequestException(errorMessage);
        }

        return await _service.GetInfraWeatherAsync(latitude, longitude);
    }

    /// <summary>
    /// Gets weather infra model from a random location.
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<WeatherInfraResponse> CurrentRandom()
    {
        var random = new Random();
        var latitude = (random.NextDouble() * 180 - 90);   // -90 to 90
        var longitude = (random.NextDouble() * 360 - 180); // -180 to 180
        return await _service.GetInfraWeatherAsync(latitude, longitude);
    }
}