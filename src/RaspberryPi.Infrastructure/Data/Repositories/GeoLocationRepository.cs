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

        public async Task<GeoLocation?> GetByPostalCodeAsync(string countryCode, string postalCode)
        {
            // TODO: find better string ordnial ignroe case comparison for sqlite
            // countryCode.ToUpperInvariant() == x.CountryCode.ToUpperInvariant()
            var geoLocation = await _dbSet.AsNoTracking()
                              .FirstOrDefaultAsync(x => string.Equals(countryCode, x.CountryCode) &&
                                                        string.Equals(postalCode, x.PostalCode));


            //var geoLocation = await _dbSet.AsNoTracking()
            //                              .FirstOrDefaultAsync(x => countryCode.ToUpperInvariant() == x.CountryCode.ToUpperInvariant() &&
            //                                                        postalCode.ToUpperInvariant() == x.PostalCode.ToUpperInvariant());

            return geoLocation;
        }
    }
}