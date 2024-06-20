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
        private readonly IFactService _factsService;
        private readonly IFactRepository _repository;
        private readonly IMapper _mapper;

        public FactAppService(IFactService factsService, IFactRepository repository, IMapper mapper)
        {
            _factsService = factsService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<FactInfraDto> GetRawRandomFactAsync()
        {
            var fact = await _factsService.GetRandomFactAsync();
            return fact;
        }

        public async Task<IEnumerable<Fact>> GetAllDatabaseFactsAsync()
        {
            return await _repository.GetAllDatabaseFactsAsync();
        }

        public async Task<FactInfraDto> SaveFactAndComputeHashAsync()
        {
            var factResponse = await _factsService.GetRandomFactAsync();
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
            foreach (var fact in facts)
            {
                var dbFact = await _repository.GetByIdAsync(fact.Id);
                if (dbFact is not null)
                {
                    throw new InvalidOperationException($"There is already a fact ID '{dbFact.Id}' in database");
                }
            }

            // TODO: add range instead
            foreach (var fact in facts)
            {
                await _repository.AddAsync(fact);
            }

            await _repository.SaveChangesAsync();
            return facts.Count();
        }

        public async Task<Fact?> GetFirstOrDefaultFactFromDatabaseAsync()
        {
            return await _repository.GetFirstOrDefaultAsync();
        }
    }
}