using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Extensions;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Services;

public class FeedbackAppService : IFeedbackAppService
{
    private readonly IEmailAppService _emailAppService;
    private readonly IGeoLocationAppService _locationAppService;
    private readonly IFeedbackMessageRepository _repository;
    private readonly ILogger _logger;

    public FeedbackAppService(IGeoLocationAppService appService,
                              IFeedbackMessageRepository repository,
                              IEmailAppService emailAppService,
                              ILogger<FeedbackAppService> logger)

    {
        _emailAppService = emailAppService;
        _locationAppService = appService;
        _repository = repository;
        _logger = logger;
    }

    public async Task SubmitFeedbackAsync(string message, string? ipAddress, string? httpHeaders)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

        GeoLocation? lookup = null;
        if (!string.IsNullOrWhiteSpace(ipAddress))
        {
            try
            {
                lookup = await _locationAppService.LookUpAsync(ipAddress);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        FeedbackMessageEntity feedback = lookup is not null
                ? new FeedbackMessageEntity
                {
                    Message = message,
                    CountryCode = lookup.CountryCode,
                    PostalCode = lookup.PostalCode,
                    RegionName = lookup.LocationName,
                    HttpHeaders = httpHeaders,
                    CreatedAtUTC = DateTime.UtcNow,
                }
                : new FeedbackMessageEntity
                {
                    Message = message,
                    HttpHeaders = httpHeaders,
                    CreatedAtUTC = DateTime.UtcNow,
                };

        await _repository.AddAsync(feedback);

        var email = new EmailDto()
        {
            To = "bruno.campiol@gmail.com",
            Subject = "New feedback received",
            Body = $"{message} <br/><br/> {feedback.ToJson()}"
        };
        await _emailAppService.TrySendEmailAsync(email);
    }

    public async Task<int> ImportBackupAsync(IEnumerable<FeedbackMessageEntity> messages)
    {
        var messageIds = messages.Select(e => e.Id).ToList();
        var messagesInDb = await _repository.GetAllAsync(g => messageIds.Contains(g.Id));
        var existingIds = messagesInDb.Select(e => e.Id).ToList();

        if (existingIds.Count > 0)
        {
            throw new InvalidOperationException(
                $"The following '{existingIds.Count}' IDs already " +
                $"exist in the database: {string.Join(", ", existingIds)}.");
        }

        await _repository.AddRangeAsync(messages);
        return messages.Count();
    }

    public async Task<IEnumerable<FeedbackMessageEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.RemoveAsync(id);
    }

    public async Task DeleteAllAsync()
    {
        await _repository.RemoveAllAsync();
    }
}