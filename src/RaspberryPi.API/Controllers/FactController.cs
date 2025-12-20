using MethodTimer;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FactController : ControllerBase
{
    private readonly IFactAppService _service;

    public FactController(IFactAppService service)
    {
        _service = service;
    }

    [Time]
    [HttpGet]
    public async Task<IEnumerable<Fact>> GetAllDatabaseFacts()
    {
        var result = await _service.GetAllDatabaseFactsAsync();
        return result;
    }

    [Time]
    [HttpGet]
    public async Task<Fact?> GetFirstOrDefaultFactFromDatabase()
    {
        var result = await _service.GetFirstOrDefaultFactFromDatabaseAsync();
        return result;
    }

    [Time]
    [HttpGet]
    public async Task<long> CountAllDatabaseFacts()
    {
        var result = await _service.CountAllDatabaseFacts();
        return result;
    }
}