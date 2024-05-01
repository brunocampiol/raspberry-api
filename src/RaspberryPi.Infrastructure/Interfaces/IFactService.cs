using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IFactService
    {
        Task<FactResponse> GetRandomFactAsync();
    }
}