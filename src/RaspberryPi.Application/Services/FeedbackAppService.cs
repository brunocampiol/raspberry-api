using Fetchgoods.Text.Json.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Models.Entity;
using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Application.Services
{
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
            FeedbackMessage feedback = null;
            IpGeoLocationInfraDetails lookup = null;

            if (ipAddress is not null)
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

            if (lookup is not null)
            {
                feedback = new FeedbackMessage()
                {
                    Message = message,
                    City = lookup.City,
                    CountryCode = lookup.CountryCode,
                    PostalCode = lookup.PostalCode,
                    RegionName = lookup.RegionName,
                    HttpHeaders = httpHeaders,
                    CreatedAtUTC = DateTime.UtcNow,
                };
            }
            else
            {
                feedback = new FeedbackMessage()
                {
                    Message = message,
                    HttpHeaders = httpHeaders,
                    CreatedAtUTC = DateTime.UtcNow,
                };
            }

            await _repository.AddAsync(feedback);
            await _repository.SaveChangesAsync();

            var email = new EmailDto()
            {
                To = "bruno.campiol@gmail.com",
                Subject = "New feedback received",
                Body = $"{message} <br/><br/> {feedback.ToJson()}"
            };
            await _emailAppService.TrySendEmailAsync(email);
        }

        public async Task<IEnumerable<FeedbackMessage>> GetAllAsync()
        {
            return await _repository.GetAll().AsNoTracking().ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            _repository.Remove(id);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            await _repository.RemoveAllAsync();
            await _repository.SaveChangesAsync();
        }
    }
}