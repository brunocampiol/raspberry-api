using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Interfaces;

public interface IFeedbackAppService
{
    Task SubmitFeedbackAsync(string message, string? ipAddress, string? httpHeaders);
    Task<IEnumerable<FeedbackMessageEntity>> GetAllAsync();
    Task<int> ImportBackupAsync(IEnumerable<FeedbackMessageEntity> messages);
    Task DeleteAsync(Guid id);
    Task DeleteAllAsync();
}