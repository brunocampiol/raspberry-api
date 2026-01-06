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
    public async Task<IEnumerable<Fact>> All()
    {
        var result = await _service.GetAllFactsAsync();
        return result;
    }

    [Time]
    [HttpGet]
    public async Task<Fact?> FirstOrDefault()
    {
        var result = await _service.GetFirstOrDefaultFactAsync();
        return result;
    }

    [Time]
    [HttpGet]
    public async Task<long> CountAll()
    {
        var result = await _service.CountAllFactsAsync();
        return result;
    }
}