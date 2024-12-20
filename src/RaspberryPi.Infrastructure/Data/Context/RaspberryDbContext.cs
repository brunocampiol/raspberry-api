using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Infrastructure.Data.Context
{
    public class RaspberryDbContext : DbContext
    {
        public DbSet<Fact> Facts { get; set; }
        public DbSet<GeoLocation> GeoLocations { get; set; }
        public DbSet<EmailOutbox> EmailsOutbox { get; set; }

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