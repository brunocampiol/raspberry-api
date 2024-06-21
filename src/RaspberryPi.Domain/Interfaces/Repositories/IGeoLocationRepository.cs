using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Domain.Interfaces.Repositories
{
    public interface IGeoLocationRepository : IRepository<GeoLocation>
    {
        Task<GeoLocation?> GetByPostalCodeAsync(string countryCode, string postalCode);
    }
}
