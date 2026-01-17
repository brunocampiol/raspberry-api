using MethodTimer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.API.Services;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Extensions;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace RaspberryPi.API.Controllers;

/// <summary>
/// Developer related methods
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class InternalController : ControllerBase
{
    private readonly IWebHostEnvironment _hostEnv;
    private readonly IInternalAppService _internalAppService;
    private readonly RequestCounterService _requestCounterService;

    public InternalController(IWebHostEnvironment hostEnv,
                              IInternalAppService internalAppService,
                              RequestCounterService requestCounterService)
    {
        _hostEnv = hostEnv ?? throw new ArgumentNullException(nameof(hostEnv));
        _internalAppService = internalAppService ?? throw new ArgumentNullException(nameof(internalAppService));
        _requestCounterService = requestCounterService ?? throw new ArgumentNullException(nameof(requestCounterService));
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

    [Time]
    [HttpGet]
    public async Task<IActionResult> GenerateDatabaseBackup()
    {
        var json = await _internalAppService.GenerateDatabaseBackupAsJsonStringAsync();

        // Set response headers for file download
        var contentDisposition = new ContentDispositionHeaderValue("attachment")
        {
            FileName = $"database-backup-{DateTime.UtcNow:yyyy-MM-dd}.json"
        };
        Response.Headers.Append("Content-Disposition", contentDisposition.ToString());
        Response.Headers.Append("Content-Type", "application/json");

        var bytes = Encoding.UTF8.GetBytes(json);
        return File(bytes, "application/json");
    }

    [Time]
    [HttpPost]
    [Authorize(Roles = "root")]
    public async Task<IActionResult> ImportDatabaseBackup(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File not selected or empty.");
        }

        using (var streamReader = new StreamReader(file.OpenReadStream()))
        {
            var json = await streamReader.ReadToEndAsync();            
            var backup = json.FromJson<DbBackupDto>();

            if (backup is null)
            {
                return BadRequest($"Invalid desserialization for '{json}'");
            }

            var importResult = await _internalAppService.ImportDatabaseBackupAsync(backup);
            return Ok($"Imported '{importResult}' rows");
        }
    }
}