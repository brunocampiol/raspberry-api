using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models;
using RaspberryPi.Infrastructure.Data.Context;

namespace RaspberryPi.Infrastructure.Data.Repositories
{
    public class AnonymousCommentRepository : IAnonymousCommentRepository
    {
        private readonly RaspberryContext _context;

        public AnonymousCommentRepository(RaspberryContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(AnonymousComment entity)
        {
            _context.Add(entity);
        }

        public void Remove(AnonymousComment entity)
        {
            _context.Remove(entity);
        }

        public AnonymousComment? GetById(Guid id)
        {
            return _context.AnonymousComments.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<AnonymousComment> GetList()
        {
            return _context.AnonymousComments.AsEnumerable();
        }

        public void Update(AnonymousComment entity)
        {
            _context.Update(entity);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}