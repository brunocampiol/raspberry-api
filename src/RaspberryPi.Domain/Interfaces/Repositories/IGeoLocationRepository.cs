using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Domain.Interfaces.Repositories
{
    public interface IGeoLocationRepository : IRepository<GeoLocation>
    {
        Task<GeoLocation?> GetAsync(string countryCode, double latitude, double longitude);
    }
}