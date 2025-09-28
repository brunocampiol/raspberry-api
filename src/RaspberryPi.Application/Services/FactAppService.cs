using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public FactAppService(IFactInfraService factsService, IFactRepository repository, IMapper mapper)
        {
            _factsInfraService = factsService;
            _repository = repository;
            _mapper = mapper;
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

            var existingIds = await _repository.GetAll()
                                        .Where(g => facts.Select(gl => gl.Id).Contains(g.Id))
                                        .Select(g => g.Id)
                                        .ToListAsync();

            if (existingIds.Count > 0)
            {
                throw new InvalidOperationException($"There are already IDs: '{string.Join(", ", existingIds)}' in database");
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