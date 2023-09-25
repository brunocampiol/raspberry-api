using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Models;

namespace RaspberryPi.Domain.Data
{
    public class RaspberryContext : DbContext, IUnitOfWork
    {
        public DbSet<AspNetUser> AspNetUsers { get; set; }

        public RaspberryContext(DbContextOptions<RaspberryContext> options)
            : base(options)
        {
        }

        public bool Commit()
        {
            return SaveChanges() > 0;
        }

        public Task<bool> CommitAsync()
        {
            // SQLite should not use async
            throw new NotSupportedException();
        }
    }
}