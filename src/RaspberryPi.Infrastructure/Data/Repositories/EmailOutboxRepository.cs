﻿using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Data.Context;

namespace RaspberryPi.Infrastructure.Data.Repositories
{
    public class EmailOutboxRepository : Repository<EmailOutbox>, IEmailOutboxRepository
    {
        public EmailOutboxRepository(RaspberryDbContext context)
            : base(context)
        {
        }
    }
}