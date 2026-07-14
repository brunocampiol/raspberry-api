using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Extensions;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Services;

public sealed class FactAppService : IFactAppService
{
    private readonly IFactRepository _repository;

    public FactAppService(IFactRepository repository)
    {
        _repository = repository;
    }

    public async Task<FactEntity?> AddAsync(string factText, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(factText);

        var fact = new FactEntity
        {
            CreatedAt = DateTime.UtcNow,
            Text = factText,
            TextHash = factText.ToSHA256Hash()
        };
        if (!await _repository.HashExistsAsync(fact.TextHash, cancellationToken))
        {
            await _repository.AddAsync(fact, cancellationToken);
            return fact;
        }

        return null;
    }

    public async Task<IEnumerable<FactEntity>> GetAllFactsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(null, cancellationToken);
    }

    public async Task<long> CountAllFactsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.CountAllDatabaseFacts(cancellationToken);
    }

    public async Task<int> ImportBackupAsync(IEnumerable<FactEntity> facts, CancellationToken cancellationToken = default)
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

    public async Task<FactEntity?> GetFirstOrDefaultFactAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetFirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> GetRandomFactAsync(CancellationToken cancellationToken = default)
    {
        var fact = await _repository.GetRandomFactAsync(cancellationToken);
        return fact?.Text;
    }

    public async Task<PagedResult<FactEntity>> SearchAsync(FactQuery query, CancellationToken cancellationToken = default)
    {
        return await _repository.SearchAsync(query, cancellationToken);
    }
}