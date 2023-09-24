using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Data;

namespace RaspberryPi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DbContextController : ControllerBase
    {
        private readonly RaspberryContext _context;

        public DbContextController(RaspberryContext context)
        {
            _context = context;
        }

        [HttpPost("migrate")]
        public void Post()
        {
            _context.Database.Migrate();
        }
    }
}