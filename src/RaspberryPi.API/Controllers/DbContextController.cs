using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaspberryPi.Infrastructure.Data.Context;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// EF Core related methods
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
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
        [Authorize(Roles = "root")]
        [HttpPost]
        public void Migrate()
        {
            _context.Database.Migrate();
        }
    }
}