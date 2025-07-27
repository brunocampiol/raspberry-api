using MethodTimer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.API.Models;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.API.Services;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Core;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;

namespace RaspberryPi.API.Controllers;

/// <summary>
/// Developer related methods
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class InternalController : ControllerBase
{
    private readonly IWebHostEnvironment _hostEnv;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IFactAppService _factAppService;
    private readonly IGeoLocationAppService _geolocationAppService;
    private readonly RequestCounterService _requestCounterService;

    public InternalController(IWebHostEnvironment hostEnv, 
                              IHttpClientFactory httpClientFactory,
                              IFactAppService factAppService,
                              IGeoLocationAppService geolocationAppService,
                              RequestCounterService requestCounterService)
    {
        _hostEnv = hostEnv ?? throw new ArgumentNullException(nameof(hostEnv));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _factAppService = factAppService ?? throw new ArgumentNullException(nameof(factAppService));
        _geolocationAppService = geolocationAppService ?? throw new ArgumentNullException(nameof(geolocationAppService));
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
    public IActionResult Settings()
    {
        string? frameworkName = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
        string? aspNetCoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        string osDescription = $"{RuntimeInformation.OSDescription} [ {RuntimeInformation.OSArchitecture} ]";

        var settings = new
        {
            EnvironmentName = _hostEnv.EnvironmentName,
            UserName = Environment.UserName,
            ASPNetCore_Environment = aspNetCoreEnv,
            OSDescription = osDescription,
            FrameworkName = frameworkName,
            IsDevelopment = _hostEnv.IsDevelopment(),
            IsProduction = _hostEnv.IsProduction(),
        };

        return Ok(settings);
    }

    [Time]
    [HttpGet]
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
    public IActionResult EchoHeaders()
    {
        var headers = Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString())
                                     .OrderBy(x => x.Key);

        return Ok(headers);
    }

    [Time]
    [HttpGet]
    public IActionResult EchoIpAddress()
    {
        return Ok(HttpContext.Connection.RemoteIpAddress?.ToString());
    }

    [Time]
    [HttpGet]
    public IActionResult EchoCookies()
    {
        var cookies = Request.Cookies.ToDictionary(x => x.Key, x => x.Value.ToString())
                                     .OrderBy(x => x.Key);

        return Ok(cookies);
    }

    [Time]
    [HttpGet]
    public async Task<Result<string>> WwwGetGoogle()
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromSeconds(15);

        var uri = new Uri("http://www.google.com");
        var httpResponse = await httpClient.GetAsync(uri);
        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = $"Response '{httpResponse.StatusCode}' " +
                               $"is not in 2XX range: '{httpContent}'";

            return Result<string>.Failure(errorMessage);
        }

        return Result<string>.Success(httpContent);
    }

    [Time]
    [HttpGet]
    [Authorize(Roles = "root")]
    public async Task<Result<string>> WwwGetAsString([FromHeader][Required] string url,
                                                     [FromHeader][Required] int timeoutInSeconds = 15)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);

        var uri = new Uri(url);
        var httpResponse = await httpClient.GetAsync(uri);
        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = $"Response '{httpResponse.StatusCode}' " +
                               $"is not in 2XX range: '{httpContent}'";

            return Result<string>.Failure(errorMessage);
        }

        return Result<string>.Success(httpContent);
    }

    [Time]
    [HttpGet]
    public async Task<IActionResult> GenerateDatabaseBackup()
    {
        var dbFacts = await _factAppService.GetAllDatabaseFactsAsync();
        var dbGeolocations = await _geolocationAppService.GetAllGeoLocationsFromDatabaseAsync();
        var dbBackup = new BackupViewModel
        {
            Facts = dbFacts,
            GeoLocations = dbGeolocations
        };

        // Serialize the data to JSON
        // TODO: use json serialization extension instead
        var json = JsonSerializer.Serialize(dbBackup, new JsonSerializerOptions { WriteIndented = true });

        // Set response headers for file download
        var contentDisposition = new ContentDispositionHeaderValue("attachment")
        {
            FileName = $"database-backup-{DateTime.UtcNow.ToString("yyyy-MM-dd")}.json"
        };
        Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
        Response.Headers.Add("Content-Type", "application/json");

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
            // TODO: use json serialization extension instead
            var backup = JsonSerializer.Deserialize<BackupViewModel>(json);
            if  (backup is null)
            {
                return BadRequest($"Invalid desserialization for '{json}'");
            }

            var geoLocationImportCount = await _geolocationAppService.ImportBackupAsync(backup.GeoLocations);
            var factImportCount = await _factAppService.ImportBackupAsync(backup.Facts);

            return Ok($"Imported {geoLocationImportCount + factImportCount} rows");
        }
    }
}