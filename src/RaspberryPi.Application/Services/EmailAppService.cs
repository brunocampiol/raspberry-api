using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Emails;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.Application.Services;

public class EmailAppService : IEmailAppService
{
    private readonly EmailOptions _settings;
    private readonly IEmailInfraService _infraService;
    private readonly IEmailOutboxRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public EmailAppService(IOptions<EmailOptions> settings,
                           ILogger<EmailAppService> logger,
                           IEmailInfraService infraService,
                           IEmailOutboxRepository repository,
                           IMapper mapper)
    {
        _settings = settings.Value;
        _infraService = infraService;
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmailOutbox>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<EmailOutbox?> GetLastSentEmailAsync()
    {
        return await _repository.GetLastSentEmailAsync();
    }

    public async Task<EmailOutbox> SendEmailAsync(EmailDto emailDto)
    {
        var email = _mapper.Map<Email>(emailDto);
        await _infraService.SendEmailAsync(email);

        var sentEmail = new EmailOutbox
        {
            Id = Guid.NewGuid(),
            From = _settings.FromEmail,
            To = emailDto.To,
            Subject = emailDto.Subject,
            Body = emailDto.Body,
            SentAtUTC = DateTime.UtcNow
        };

        await _repository.AddAsync(sentEmail);
        return sentEmail;
    }

    public async Task SendEmailInfraAsync(EmailDto emailDto)
    {
        var email = _mapper.Map<Email>(emailDto);
        await _infraService.SendEmailAsync(email);
    }

    public async Task<int> ImportBackupAsync(IEnumerable<EmailOutbox> emails)
    {
        var emailIds = emails.Select(e => e.Id).ToList();
        var emailsInDb = await _repository.GetAllAsync(g => emailIds.Contains(g.Id));
        var existingIds = emailsInDb.Select(e => e.Id).ToList();

        if (existingIds.Count > 0)
        {
            throw new InvalidOperationException(
                $"The following '{existingIds.Count}' IDs already " +
                $"exist in the database: {string.Join(", ", existingIds)}.");
        }

        await _repository.AddRangeAsync(emails);
        return emails.Count();
    }

    public async Task TrySendEmailAsync(EmailDto emailDto)
    {
        try
        {
            await SendEmailAsync(emailDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    public async Task DeleteAllAsync()
    {
        await _repository.RemoveAllAsync();
    }
}