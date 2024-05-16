using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaspberryPi.Infrastructure.Data.Context;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// EF Core related methods
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DbContextController : ControllerBase
    {
        private readonly RaspberryDbContext _context;

        public DbContextController(RaspberryDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        [HttpPost("migrate")]
        public void Post()
        {
            _context.Database.Migrate();
        }
    }
}