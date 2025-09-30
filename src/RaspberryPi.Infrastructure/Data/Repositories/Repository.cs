using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Data.Context;

namespace RaspberryPi.Infrastructure.Data.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly RaspberryDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected Repository(RaspberryDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public virtual void Remove(Guid id)
    {
        _dbSet.Remove(_dbSet.Find(id) ?? throw new KeyNotFoundException($"Could not find ID '{id}'"));
    }

    public virtual async Task RemoveAllAsync()
    {
        await _dbSet.ExecuteDeleteAsync();
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual IQueryable<T> GetAll()
    {
        return _dbSet;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _context.Dispose();
    }
}