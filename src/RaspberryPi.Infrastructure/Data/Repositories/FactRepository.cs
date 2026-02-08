using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Helpers;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Domain.Specifications;
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

    public async Task<PagedResult<Fact>> SearchAsync(FactQuery query, CancellationToken cancellationToken = default)
    {
        var spec = new FactsSearchSpec(query);
        var facts = _dbSet.AsQueryable();

        IQueryable<Fact> countQuery = facts;
        if (spec.AsNoTracking) countQuery = countQuery.AsNoTracking();
        if (spec.Criteria is not null) countQuery = countQuery.Where(spec.Criteria);

        var total = await countQuery.CountAsync(cancellationToken);
        var pagedQuery = SpecificationEvaluator.GetQuery(facts, spec);
        var items = await pagedQuery.ToListAsync(cancellationToken);

        return new PagedResult<Fact>(query.Page, query.PageSize, total, items);
    }
}