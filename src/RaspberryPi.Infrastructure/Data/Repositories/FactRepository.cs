using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Data.Context;

namespace RaspberryPi.Infrastructure.Data.Repositories;

public class FactRepository : Repository<Fact>, IFactRepository
{
    public FactRepository(RaspberryDbContext context)
        : base(context)
    {
    }

    public async Task<long> CountAllDatabaseFacts(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().LongCountAsync(cancellationToken);
    }

    public async Task<Fact?> GetFirstOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> HashExistsAsync(string hashValue, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.TextHash == hashValue, cancellationToken);
    }
}