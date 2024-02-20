using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Infrastructure.Data.EFCore.Context
{
    public class RaspberryContext : DbContext, IUnitOfWork
    {
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AnonymousComment> AnonymousComments { get; set; }

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