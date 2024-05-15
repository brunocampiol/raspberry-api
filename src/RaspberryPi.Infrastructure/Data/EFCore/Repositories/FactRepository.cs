using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Data.EFCore.Context;

namespace RaspberryPi.Infrastructure.Data.EFCore.Repositories
{
    public class FactRepository : Repository<Fact>, IFactRepository
    {
        public FactRepository(RaspberryDbContext context)
            : base(context)
        {
        }

        public async Task<long> CountAllDatabaseFacts()
        {
            return await _dbSet.AsNoTracking().LongCountAsync();
        }

        public async Task<IEnumerable<Fact>> GetAllDatabaseFactsAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<bool> HashExistsAsync(string hashValue)
        {
            return await _dbSet.AnyAsync(x => x.TextHash == hashValue);
        }
    }
}