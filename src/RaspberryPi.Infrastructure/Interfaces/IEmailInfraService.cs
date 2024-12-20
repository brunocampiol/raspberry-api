using RaspberryPi.Infrastructure.Models.Emails;

namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IEmailInfraService
    {
        Task<Guid> SendEmailAsync(Email email);
    }
}