using MethodTimer;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Extensions;
using RaspberryPi.API.Mapping;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Interfaces;

namespace RaspberryPi.API.Controllers;

/// <summary>
/// Mainpoint for https://brunocampiol.github.io/
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class WebsiteController : ControllerBase
{
    private readonly IWebsiteAppService _service;

    public WebsiteController(IWebsiteAppService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Retrieves weather information for home page.
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<WeatherViewModel> Weather()
    {
        var clientIp = HttpContext.GetClientIpAddress();
        var result = await _service.GetWeatherAsync(clientIp);
        var viewModel = result.MapToWeatherViewModel();
        return viewModel;
    }

    /// <summary>
    /// Retrieves a fact for home page.
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<FactViewModel> Fact()
    {
        var result = await _service.FetchAndStoreUniqueFactAsync();
        var viewModel = result.MapToFactViewModel();
        return viewModel;
    }
}