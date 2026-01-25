using MethodTimer;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.API.Services;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Core;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace RaspberryPi.API.Controllers;

/// <summary>
/// Developer related methods
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class InternalController : ControllerBase
{
    private readonly IWebHostEnvironment _hostEnv;
    private readonly RequestCounterService _requestCounterService;

    public InternalController(IWebHostEnvironment hostEnv,
                              IDatabaseAppService internalAppService,
                              RequestCounterService requestCounterService)
    {
        _hostEnv = hostEnv ?? 
            throw new ArgumentNullException(nameof(hostEnv));
        _requestCounterService = requestCounterService ?? 
            throw new ArgumentNullException(nameof(requestCounterService));
    }

    [Time]
    [HttpGet]
    public IReadOnlyDictionary<string, EndpointDetail> RequestCounts()
    {
        return _requestCounterService.GetAll();
    }

    [Time]
    [HttpGet]
    public SettingsResponseViewModel Settings()
    {
        string? frameworkName = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
        string? aspNetCoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        string osDescription = $"{RuntimeInformation.OSDescription} [ {RuntimeInformation.OSArchitecture} ]";

        var settings = new SettingsResponseViewModel
        {
            EnvironmentName = _hostEnv.EnvironmentName,
            UserName = Environment.UserName,
            ASPNetCore_Environment = aspNetCoreEnv,
            OSDescription = osDescription,
            FrameworkName = frameworkName,
            IsDevelopment = _hostEnv.IsDevelopment(),
            IsProduction = _hostEnv.IsProduction(),
        };

        return settings;
    }

    [Time]
    [HttpGet]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public IActionResult Timestamp()
    {
        return Ok(DateTime.Now);
    }

    [Time]
    [HttpPost]
    public IActionResult Exception(string? exceptionMessage)
    {
        throw new AppException(exceptionMessage);
    }

    [Time]
    [HttpGet]
    public IDictionary<string, string> EchoHeaders()
    {
        return Request.Headers.OrderBy(x => x.Key)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.ToString() ?? string.Empty
                );
    }

    [Time]
    [HttpGet]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public IActionResult EchoIpAddress()
    {
        return Ok(HttpContext.Connection.RemoteIpAddress?.ToString());
    }

    [Time]
    [HttpGet]
    public IDictionary<string, string> EchoCookies()
    {
        return Request.Cookies.OrderBy(x => x.Key)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.ToString() ?? string.Empty
                );
    }
}