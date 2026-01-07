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

    /// <summary>
    /// Retrieves all database facts.
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<IEnumerable<Fact>> All()
    {
        var result = await _service.GetAllFactsAsync();
        return result;
    }

    /// <summary>
    /// Retrieves the first available fact from the database, if any.
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<Fact?> FirstOrDefault()
    {
        var result = await _service.GetFirstOrDefaultFactAsync();
        return result;
    }

    /// <summary>
    /// Gets the total number of facts available in the data store.
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<long> TotalCount()
    {
        var result = await _service.CountAllFactsAsync();
        return result;
    }
}