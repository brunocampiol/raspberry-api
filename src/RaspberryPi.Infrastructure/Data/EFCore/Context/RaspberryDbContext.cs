using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Infrastructure.Data.EFCore.Context
{
    public class RaspberryDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Fact> Facts { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }

        public RaspberryDbContext(DbContextOptions<RaspberryDbContext> options)
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