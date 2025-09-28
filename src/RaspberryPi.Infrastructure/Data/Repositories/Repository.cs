using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Data.Context;

namespace RaspberryPi.Infrastructure.Data.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly RaspberryDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(RaspberryDbContext context)
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
        _dbSet.Remove(_dbSet.Find(id) ?? throw new Exception("Could not find ID '{id}'"));
    }

    public virtual async Task RemoveAllAsync()
    {
        // TODO: use truncate/delete
        // https://www.codeproject.com/Articles/5339402/Delete-All-Rows-in-Entity-Framework-Core-6
        // https://juldhais.net/how-to-update-and-delete-multiple-rows-in-entity-framework-core-c4068304295d

        // Currently not performatic. It does delete one at a time
        _dbSet.RemoveRange(await _dbSet.ToListAsync());
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
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}