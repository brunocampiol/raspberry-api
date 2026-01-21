using Microsoft.EntityFrameworkCore;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Data.Context;

namespace RaspberryPi.Infrastructure.Data.Repositories;

public class EmailOutboxRepository : Repository<EmailOutbox>, IEmailOutboxRepository
{
    public EmailOutboxRepository(RaspberryDbContext context)
        : base(context)
    {
    }

    public async Task<EmailOutbox?> GetLastSentEmailAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
                        .AsNoTracking()
                        .OrderByDescending(e => e.SentAtUTC)
                        .FirstOrDefaultAsync(cancellationToken);
    }
}