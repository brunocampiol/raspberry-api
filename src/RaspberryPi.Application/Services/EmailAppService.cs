using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Emails;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.Application.Services
{
    public class EmailAppService : IEmailAppService
    {
        private readonly EmailOptions _settings;
        private readonly IEmailInfraService _infraService;
        private readonly IEmailOutboxRepository _repository;
        private readonly IMapper _mapper;


        public EmailAppService(IOptions<EmailOptions> settings,
                               IEmailInfraService infraService,
                               IEmailOutboxRepository repository,
                               IMapper mapper)
        {
            _settings = settings.Value;
            _infraService = infraService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmailOutbox>> GetAllAsync()
        {
            return await _repository.GetAll().AsNoTracking().ToListAsync();
        }

        public async Task<EmailOutbox> SendEmailAsync(EmailDto email)
        {
            var mailjetEmail = _mapper.Map<Email>(email);
            var responseId = await _infraService.SendEmailAsync(mailjetEmail);

            var sentEmail = new EmailOutbox
            {
                Id = responseId,
                From = _settings.FromEmail,
                To = email.To,
                Subject = email.Subject,
                Body = email.Body,
                SentAtUTC = DateTime.UtcNow
            };

            await _repository.AddAsync(sentEmail);
            await _repository.SaveChangesAsync();
            return sentEmail;
        }
    }
}