using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Extensions;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Application.Services;

public sealed class FactAppService : IFactAppService
{
    private readonly IFactInfraService _infraService;
    private readonly IFactRepository _repository;

    public FactAppService(IFactInfraService factsService, IFactRepository repository)
    {
        _infraService = factsService;
        _repository = repository;
    }

    public async Task<FactInfraResponse> FetchFactAsync(CancellationToken cancellationToken = default)
    {
        var fact = await _infraService.GetRandomFactAsync(cancellationToken);
        return fact;
    }

    public async Task<IEnumerable<Fact>> GetAllFactsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(null, cancellationToken);
    }

    public async Task<FactInfraResponse> FetchAndStoreUniqueFactAsync(CancellationToken cancellationToken = default)
    {
        var factResponse = await _infraService.GetRandomFactAsync(cancellationToken);
        var fact = new Fact
        {
            CreatedAt = DateTime.UtcNow,
            Text = factResponse.Text,
            TextHash = factResponse.Text.ToSHA256Hash()
        };

        if (!await _repository.HashExistsAsync(fact.TextHash, cancellationToken))
        {
            await _repository.AddAsync(fact, cancellationToken);
        }

        return factResponse;
    }

    public async Task<long> CountAllFactsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.CountAllDatabaseFacts(cancellationToken);
    }

    public async Task<int> ImportBackupAsync(IEnumerable<Fact> facts, CancellationToken cancellationToken = default)
    {
        var factIds = facts.Select(e => e.Id).ToList();
        var factsInDb = await _repository.GetAllAsync(g => factIds.Contains(g.Id), cancellationToken);
        var existingIds = factsInDb.Select(e => e.Id).ToList();

        if (existingIds.Count > 0)
        {
            throw new InvalidOperationException(
                $"The following '{existingIds.Count}' IDs already " +
                $"exist in the database: {string.Join(", ", existingIds)}.");
        }

        await _repository.AddRangeAsync(facts, cancellationToken);
        return facts.Count();
    }

    public async Task<Fact?> GetFirstOrDefaultFactAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetFirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<Fact>> SearchAsync(FactQuery query, CancellationToken cancellationToken = default)
    {
        return await _repository.SearchAsync(query, cancellationToken);
    }
}