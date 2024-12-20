using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Data.Context;

namespace RaspberryPi.Infrastructure.Data.Repositories
{
    public class FeedbackMessageRepository : Repository<FeedbackMessage>, IFeedbackMessageRepository
    {
        public FeedbackMessageRepository(RaspberryDbContext context)
            : base(context)
        {
        }
    }
}