using MethodTimer;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;

namespace RaspberryPi.API.Controllers;

/// <summary>
/// Raspberry Pi 2 and hardware specific methods
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class HardwareController : ControllerBase
{
    // TODO: update to libre hardware monitor
    // https://github.com/LibreHardwareMonitor/LibreHardwareMonitor

    private readonly IHardwareAppService _service;

    public HardwareController(IHardwareAppService hardwareAppService)
    {
        _service = hardwareAppService;
    }

    /// <summary>
    /// Blinks a LED in GPIO 26
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult BlinkLedGpio26()
    {
        _service.BlinkLedGpio26();
        return NoContent();
    }

    /// <summary>
    /// Reads GPIO 26
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public IActionResult ReadGpio26()
    {
        var result = _service.ReadGpio26();
        return Ok(result);
    }
}
