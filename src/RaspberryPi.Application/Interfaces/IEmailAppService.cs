using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Interfaces;

public interface IEmailAppService
{
    Task<IEnumerable<EmailOutbox>> GetAllAsync();
    Task<EmailOutbox?> GetLastSentEmailAsync();
    Task<EmailOutbox> SendEmailAsync(EmailDto emailDto);
    Task SendEmailInfraAsync(EmailDto emailDto);
    Task<int> ImportBackupAsync(IEnumerable<EmailOutbox> emails);
    Task TrySendEmailAsync(EmailDto emailDto);
    Task DeleteAllAsync();
}