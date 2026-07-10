using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Infrastructure.Data.Context
{
    public class RaspberryDbContext : DbContext
    {
        public DbSet<FactEntity> FactEntities { get; set; }
        public DbSet<GeoLocationEntity> GeoLocationEntities { get; set; }
        public DbSet<EmailOutboxEntity> EmailOutboxEntities { get; set; }
        public DbSet<FeedbackMessageEntity> FeedbackMessageEntities { get; set; }

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