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

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.AsNoTracking()
                           .FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public virtual async Task RemoveAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id)
          ?? throw new KeyNotFoundException($"Could not find '{typeof(T).Name}' with ID: '{id}'");

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task RemoveAllAsync()
    {
        // No need to call SaveChangesAsync
        await _dbSet.ExecuteDeleteAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null)
    {
        var query = _dbSet.AsNoTracking();

        if (predicate is not null) query = query.Where(predicate);

        return await query.ToListAsync();
    }
}