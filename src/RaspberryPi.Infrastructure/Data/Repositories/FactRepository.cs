using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Data.Context;

namespace RaspberryPi.Infrastructure.Data.Repositories
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

        public async Task<Fact?> GetFirstOrDefaultAsync()
        {
            return await _dbSet.FirstOrDefaultAsync();
        }

        public async Task<bool> HashExistsAsync(string hashValue)
        {
            return await _dbSet.AnyAsync(x => x.TextHash == hashValue);
        }
    }
}