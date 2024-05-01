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
        private readonly IFactService _factsService;
        private readonly IFactRepository _repository;

        public FactAppService(IFactService factsService, IFactRepository repository)
        {
            _factsService = factsService;
            _repository = repository;
        }

        public async Task<FactResponse> GetRawRandomFactAsync()
        {
            var fact = await _factsService.GetRandomFactAsync();
            return fact;
        }

        public async Task<IEnumerable<Fact>> GetAllDatabaseFactsAsync()
        {
            return await _repository.GetAllDatabaseFactsAsync();
        }

        public async Task<FactResponse> SaveFactAndComputeHashAsync()
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
    }
}