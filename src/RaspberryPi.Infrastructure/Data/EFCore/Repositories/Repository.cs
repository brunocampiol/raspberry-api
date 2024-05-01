using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Data.EFCore.Context;

namespace RaspberryPi.Infrastructure.Data.EFCore.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly RaspberryDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(RaspberryDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // TODO: remove this in future
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await  _dbSet.FindAsync(id);
        }

        public virtual async Task AddAsync(T entity)
        {
           await _dbSet.AddAsync(entity);
        }

        public virtual void Remove(Guid id)
        {
           _dbSet.Remove(_dbSet.Find(id));
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
}