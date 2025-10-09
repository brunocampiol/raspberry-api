using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Data.Context;

namespace RaspberryPi.Infrastructure.Data.Repositories
{
    public class GeoLocationRepository : Repository<GeoLocation>, IGeoLocationRepository
    {
        public GeoLocationRepository(RaspberryDbContext context)
            :base(context) 
        {

        }

        public async Task<GeoLocation?> GetAsync(string countryCode, double latitude, double longitude)
        {
            // Round values to 3 decimal places
            var roundedLatitude = Math.Round(latitude, 3);
            var roundedLongitude = Math.Round(longitude, 3);

            var geoLocation = await _dbSet.AsNoTracking()
                              .FirstOrDefaultAsync(x => string.Equals(countryCode, x.CountryCode) &&
                                                      x.Latitude >= roundedLatitude - 0.001 &&
                                                      x.Latitude <= roundedLatitude + 0.001 &&
                                                      x.Longitude >= roundedLongitude - 0.001 &&
                                                      x.Longitude <= roundedLongitude + 0.001);

            return geoLocation;
        }
    }
}