using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Facts;
using System.Security.Cryptography;
using System.Text;

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

        public async Task<FactResponse> GetRandomFactAsync()
        {
            var fact = await _factsService.GetRandomFactAsync();
            return fact;
        }

        public async Task<FactResponse> SaveFactAndComputeHashAsync()
        {
            var factResponse = await _factsService.GetRandomFactAsync();
            var hash = string.Empty;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(factResponse.Fact));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                hash = builder.ToString();
            }

            var fact = new Fact
            {
                CreatedAt = DateTime.UtcNow,
                Text = factResponse.Fact,
                TextHash = hash
            };

            await _repository.AddAsync(fact);
            await _repository.SaveChangesAsync();

            return factResponse;
        }
    }
}