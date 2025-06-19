﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        return await _repository.GetAll().AsNoTracking().ToListAsync();
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
        await _repository.SaveChangesAsync();
        return sentEmail;
    }

    public async Task TrySendEmailAsync(EmailDto email)
    {
        try
        {
            await SendEmailAsync(email);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    public async Task DeleteAllAsync()
    {
        await _repository.RemoveAllAsync();
        await _repository.SaveChangesAsync();
    }
}