using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Interfaces;

public interface IFeedbackAppService
{
    Task SubmitFeedbackAsync(string message, string? ipAddress, string? httpHeaders);
    Task<IEnumerable<FeedbackMessage>> GetAllAsync();
    Task<int> ImportBackupAsync(IEnumerable<FeedbackMessage> messages);
    Task DeleteAsync(Guid id);
    Task DeleteAllAsync();
}