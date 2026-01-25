using MethodTimer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Extensions;
using RaspberryPi.Infrastructure.Data.Context;
using System.Net.Http.Headers;
using System.Text;

namespace RaspberryPi.API.Controllers;

/// <summary>
/// Database support
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class DatabaseController : ControllerBase // TODO: rename this to database and move import and bkp here
{
    private readonly RaspberryDbContext _context;
    private readonly IDatabaseAppService _databaseAppService;

    public DatabaseController(RaspberryDbContext context, IDatabaseAppService databaseAppService)
    {
        _context = context;
        _databaseAppService = databaseAppService ?? 
            throw new ArgumentNullException(nameof(databaseAppService));
    }

    [Time]
    [HttpPost]
    [Authorize(Roles = "root")]
    public void Migrate()
    {
        _context.Database.Migrate();
    }

    [Time]
    [HttpGet]
    public async Task<IActionResult> GenerateBackup()
    {
        var json = await _databaseAppService.GenerateDatabaseBackupAsJsonStringAsync();

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
    public async Task<IActionResult> ImportBackup(IFormFile file)
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

            var importResult = await _databaseAppService.ImportDatabaseBackupAsync(backup);
            return Ok($"Imported '{importResult}' rows");
        }
    }
}