using Microsoft.EntityFrameworkCore;
using RaspberryPi.API.Models.Data;

namespace RaspberryPi.API.Data
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }

        public string DbPath { get; }

        public BloggingContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "database.sqlite");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            //=> options.UseSqlite($"Data Source={DbPath}");
            => options.UseSqlite($"Data Source=./database.sqlite");
    }
}