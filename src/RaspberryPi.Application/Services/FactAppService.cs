using RaspberryPi.Application.Interfaces;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Application.Services
{
    public sealed class FactAppService : IFactAppService
    {
        private readonly IFactService _factsService;

        public FactAppService(IFactService factsService)
        {
            _factsService = factsService;
        }

        public async Task<FactResponse> GetRandomFactAsync()
        {
            var fact = await _factsService.GetRandomFactAsync();
            return fact;
        }
    }
}