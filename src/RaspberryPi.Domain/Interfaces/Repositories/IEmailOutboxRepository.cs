using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Domain.Interfaces.Repositories
{
    public interface IEmailOutboxRepository : IRepository<EmailOutbox>
    {
    }
}