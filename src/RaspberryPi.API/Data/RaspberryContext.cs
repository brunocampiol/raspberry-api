using Microsoft.EntityFrameworkCore;
using RaspberryPi.API.Models.Data;

namespace RaspberryPi.API.Data
{
    public class RaspberryContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }

        public RaspberryContext(DbContextOptions<RaspberryContext> options)
            : base(options)
        {
        }
    }
}