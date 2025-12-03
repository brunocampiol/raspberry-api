using AutoMapper;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Extensions;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Application.Services
{
    public sealed class FactAppService : IFactAppService
    {
        private readonly IFactInfraService _factsInfraService;
        private readonly IFactRepository _repository;

        public FactAppService(IFactInfraService factsService, IFactRepository repository, IMapper mapper)
        {
            _factsInfraService = factsService;
            _repository = repository;
        }

        public async Task<FactInfraDto> GetRawRandomFactAsync()
        {
            var fact = await _factsInfraService.GetRandomFactAsync();
            return fact;
        }

        public async Task<IEnumerable<Fact>> GetAllDatabaseFactsAsync()
        {
            return await _repository.GetAllDatabaseFactsAsync();
        }

        public async Task<FactInfraDto> SaveFactAndComputeHashAsync()
        {
            var factResponse = await _factsInfraService.GetRandomFactAsync();
            var fact = new Fact
            {
                CreatedAt = DateTime.UtcNow,
                Text = factResponse.Fact,
                TextHash = factResponse.Fact.ToSHA256Hash()
            };

            if (!await _repository.HashExistsAsync(fact.TextHash))
            {
                await _repository.AddAsync(fact);
                await _repository.SaveChangesAsync();
            }

            return factResponse;
        }

        public async Task<long> CountAllDatabaseFacts()
        {
            return await _repository.CountAllDatabaseFacts();
        }

        public async Task<int> ImportBackupAsync(IEnumerable<Fact> facts)
        {
            var factIds = facts.Select(e => e.Id).ToList();
            var factsInDb = await _repository.GetAllAsync(g => factIds.Contains(g.Id));
            var existingIds = factsInDb.Select(e => e.Id).ToList();

            if (existingIds.Count > 0)
            {
                throw new InvalidOperationException(
                    $"The following '{existingIds.Count}' IDs already " +
                    $"exist in the database: {string.Join(", ", existingIds)}.");
            }

            await _repository.AddRangeAsync(facts);
            await _repository.SaveChangesAsync();
            return facts.Count();
        }

        public async Task<Fact?> GetFirstOrDefaultFactFromDatabaseAsync()
        {
            return await _repository.GetFirstOrDefaultAsync();
        }
    }
}