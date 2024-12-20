using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Interfaces
{
    public interface IEmailAppService
    {
        Task<IEnumerable<EmailOutbox>> GetAllAsync();
        Task<EmailOutbox> SendEmailAsync(EmailDto email);
        Task TrySendEmailAsync(EmailDto email);
    }
}