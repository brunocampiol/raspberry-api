using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Interfaces;

public interface IEmailAppService
{
    Task<IEnumerable<EmailOutboxEntity>> GetAllAsync();
    Task<EmailOutboxEntity?> GetLastSentEmailAsync();
    Task<EmailOutboxEntity> SendEmailAsync(EmailDto emailDto);
    Task SendEmailInfraAsync(EmailDto emailDto);
    Task<int> ImportBackupAsync(IEnumerable<EmailOutboxEntity> emails);
    Task TrySendEmailAsync(EmailDto emailDto);
    Task DeleteAllAsync();
}