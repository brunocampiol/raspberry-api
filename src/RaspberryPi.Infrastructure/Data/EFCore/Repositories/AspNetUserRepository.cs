using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Data.EFCore.Context;

namespace RaspberryPi.Infrastructure.Data.EFCore.Repositories
{
    public class AspNetUserRepository : IAspNetUserRepository
    {
        private readonly RaspberryContext _context;

        public AspNetUserRepository(RaspberryContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(AspNetUser AspNetUser)
        {
            _context.AspNetUsers.Add(AspNetUser);
        }

        public AspNetUser? GetNoTracking(Guid id)
        {
            return _context.AspNetUsers.AsNoTracking()
                                       .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<AspNetUser> ListNoTracking()
        {
            return _context.AspNetUsers.AsNoTracking()
                                       .AsEnumerable();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}