using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Application.Interfaces
{
    public interface IFactAppService
    {
        Task<FactResponse> GetRandomFactAsync();
    }
}