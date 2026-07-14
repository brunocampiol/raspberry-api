using MethodTimer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;
using System.ComponentModel.DataAnnotations;

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
    /// Retrieves a random fact.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<string?> Random(CancellationToken cancellationToken)
    {
        return await _service.GetRandomFactAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves all database facts.
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<IEnumerable<FactEntity>> All(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllFactsAsync(cancellationToken);
        return result;
    }

    /// <summary>
    /// Retrieves the first available fact from the database, if any.
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<FactEntity?> FirstOrDefault(CancellationToken cancellationToken)
    {
        var result = await _service.GetFirstOrDefaultFactAsync(cancellationToken);
        return result;
    }

    /// <summary>
    /// Gets the total number of facts available in the data store.
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<long> TotalCount(CancellationToken cancellationToken)
    {
        var result = await _service.CountAllFactsAsync(cancellationToken);
        return result;
    }

    /// <summary>
    /// Searches for facts in the database based on the provided query parameters, returning a paginated result set of matching facts.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Time]
    [HttpPost]
    public async Task<PagedResult<FactEntity>> Search(FactQuery query, CancellationToken cancellationToken)
    {
        return await _service.SearchAsync(query, cancellationToken);
    }

    /// <summary>
    /// Adds a new fact to the database based on the provided text.
    /// </summary>
    /// <param name="text">Required and must not be null or empty</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Time]
    [HttpPost]
    [Authorize(Roles = "root")]
    public async Task<FactEntity?> Add([Required][FromBody]string text, CancellationToken cancellationToken)
    {
        return await _service.AddAsync(text, cancellationToken);
    }
}