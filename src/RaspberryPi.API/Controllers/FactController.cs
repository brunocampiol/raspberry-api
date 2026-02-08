using MethodTimer;
using Microsoft.AspNetCore.Mvc;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.Facts;

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
    /// Retrieves a new fact.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the fetch operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="FactInfraResponse"/> with
    /// the retrieved fact infrastructure data.</returns>
    [Time]
    [HttpGet]
    public async Task<FactInfraResponse> Fetch(CancellationToken cancellationToken)
    {
        var result = await _service.FetchFactAsync(cancellationToken);
        return result;
    }

    /// <summary>
    /// Retrieves all database facts.
    /// </summary>
    /// <returns></returns>
    [Time]
    [HttpGet]
    public async Task<IEnumerable<Fact>> All(CancellationToken cancellationToken)
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
    public async Task<Fact?> FirstOrDefault(CancellationToken cancellationToken)
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
    public async Task<PagedResult<Fact>> Search(FactQuery query, CancellationToken cancellationToken)
    {
        return await _service.SearchAsync(query, cancellationToken);
    }
}