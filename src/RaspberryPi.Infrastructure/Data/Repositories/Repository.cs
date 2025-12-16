using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Data.Context;
using System.Linq.Expressions;

namespace RaspberryPi.Infrastructure.Data.Repositories;

public abstract class Repository<T> : IRepository<T>
    where T : class, IEntityBase
{
    protected readonly RaspberryDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected Repository(RaspberryDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
                           .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken)
          ?? throw new KeyNotFoundException($"Could not find '{typeof(T).Name}' with ID: '{id}'");

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task RemoveAllAsync(CancellationToken cancellationToken = default)
    {
        // ExecuteDeleteAsync supports cancellation token in EF Core 7+
        await _dbSet.ExecuteDeleteAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking();

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.ToListAsync(cancellationToken);
    }
}