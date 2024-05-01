using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Data.EFCore.Context;

namespace RaspberryPi.Infrastructure.Data.EFCore.Repositories
{
    public class FactRepository : Repository<Fact>, IFactRepository
    {
        public FactRepository(RaspberryDbContext context)
            : base(context)
        {
        }
    }
}