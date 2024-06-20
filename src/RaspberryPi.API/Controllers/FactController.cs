using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.Facts;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FactController : ControllerBase
    {
        private readonly IFactAppService _service;

        public FactController(IFactAppService service)
        {
            _service = service;   
        }

        [HttpGet]
        public async Task<IEnumerable<Fact>> GetAllDatabaseFacts()
        {
            var result = await _service.GetAllDatabaseFactsAsync();
            return result;
        }

        [HttpGet]
        public async Task<Fact?> GetFirstOrDefaultFactFromDatabase()
        {
            var result = await _service.GetFirstOrDefaultFactFromDatabaseAsync();
            return result;
        }

        [HttpGet]
        public async Task<long> CountAllDatabaseFacts()
        {
            var result = await _service.CountAllDatabaseFacts();
            return result;
        }

        [HttpGet]
        public async Task<FactResponse> SaveFactAndComputeHash()
        {
            var result = await _service.SaveFactAndComputeHashAsync();
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> ExportFactsBackup()
        {
            var result = await _service.GetAllDatabaseFactsAsync();

            // Serialize the data to JSON
            // TODO: use json serialization extension instead
            var json = JsonSerializer.Serialize(result);

            // Set response headers for file download
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "database-facts.json"
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            Response.Headers.Add("Content-Type", "application/json");

            var bytes = Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json");
        }

        [HttpPost]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> ImportFactsBackup(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File not selected or empty.");
            }

            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var json = await streamReader.ReadToEndAsync();
                // TODO: use json serialization extension instead
                var facts = JsonSerializer.Deserialize<IEnumerable<Fact>>(json);

                var importedCount = await _service.ImportBackupAsync(facts);
                return Ok($"Imported {facts.Count()} rows");
            }
        }
    }
}