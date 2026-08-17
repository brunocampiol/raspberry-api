using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Helpers;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Domain.Specifications;
using RaspberryPi.Infrastructure.Data.Context;

namespace RaspberryPi.Infrastructure.Data.Repositories;

public class FactRepository : Repository<FactEntity>, IFactRepository
{
    public FactRepository(RaspberryDbContext context)
        : base(context)
    {
    }

    public async Task<long> CountAllDatabaseFacts(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().LongCountAsync(cancellationToken);
    }

    public async Task<FactEntity?> GetFirstOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<FactEntity?> GetRandomFactAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
                           .OrderBy(x => EF.Functions.Random())
                           .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> HashExistsAsync(string hashValue, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.TextHash == hashValue, cancellationToken);
    }

    public async Task<PagedResult<FactEntity>> SearchAsync(FactQuery query, CancellationToken cancellationToken = default)
    {
        var spec = new FactsSearchSpec(query);
        var baseQuery = _dbSet.AsQueryable();
        if (spec.AsNoTracking) baseQuery = baseQuery.AsNoTracking();

        var countQuery = spec.Criteria is not null 
            ? baseQuery.Where(spec.Criteria) 
            : baseQuery;

        var total = await countQuery.CountAsync(cancellationToken);
        var pagedQuery = SpecificationEvaluator.GetQuery(baseQuery, spec);
        var items = await pagedQuery.ToListAsync(cancellationToken);

        return new PagedResult<FactEntity>(query.Page, query.PageSize, total, items);
    }
}