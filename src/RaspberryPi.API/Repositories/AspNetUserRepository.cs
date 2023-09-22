using Microsoft.EntityFrameworkCore;
using RaspberryPi.API.Data;
using RaspberryPi.API.Models.Data;

namespace RaspberryPi.API.Repositories
{
    public class AspNetUserRepository : IAspNetUserRepository
    {
        private readonly BloggingContext _context;

        public AspNetUserRepository(BloggingContext context)
        {
            _context = context;
        }

        // public IUnitOfWork UnitOfWork => _context;

        public void Add(AspNetUser AspNetUser)
        {
            _context.AspNetUsers.Add(AspNetUser);
            _context.SaveChanges();
        }

        public AspNetUser? Get(Guid id)
        {
            return _context.AspNetUsers.AsNoTracking()
                                       .FirstOrDefault(x => x.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}