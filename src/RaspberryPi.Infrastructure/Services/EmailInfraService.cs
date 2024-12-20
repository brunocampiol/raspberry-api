using Microsoft.Extensions.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Emails;
using RaspberryPi.Infrastructure.Models.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RaspberryPi.Infrastructure.Services
{
    public sealed class EmailInfraService : IEmailInfraService
    {
        // https://dev.mailjet.com/email/guides/

        private readonly EmailOptions _settings;
        private readonly IHttpClientFactory _httpClientFactory;


        public EmailInfraService(IOptions<EmailOptions> settings,
                                 IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;

            ArgumentNullException.ThrowIfNullOrWhiteSpace(_settings.APISecret);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(_settings.APIKey);
        }

        public async Task<Guid> SendEmailAsync(Email email)
        {
            var messages = new
            {
                Messages = new[]
                {
                    new
                    {
                        From = new { Email = _settings.FromEmail },
                        To = new[] { new { Email = email.To } },
                        Subject = email.Subject,
                        HTMLPart = email.Body
                    }
                }
            };

            var endpoint = "/send";
            var httpClient = _httpClientFactory.CreateClient();
            var byteArray = Encoding.ASCII.GetBytes($"{_settings.APIKey}:{_settings.APISecret}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            var uri = new Uri($"{_settings.BaseUrl}{endpoint}");
            var json = JsonSerializer.Serialize(messages);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await httpClient.PostAsync(uri, content);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorMessage = $"Failed to send email. " +
                                   $"The HTTP response '{httpResponse.StatusCode}' " +
                                   $"is not in 2XX range for '{uri}'. Received " +
                                   $"content is '{httpContent}'";
                throw new AppException(errorMessage);
            }

            var response = JsonSerializer.Deserialize<SendResponse>(httpContent);
            if (response is null) throw new AppException($"Unable to parse content '{httpContent}'");
            if (response.Messages.Count <= 0) throw new AppException($"There are no Message elements in '{httpContent}'");
            if (response.Messages[0].To.Count <= 0) throw new AppException($"There are no To elements in '{httpContent}'");

            return Guid.Parse(response.Messages[0].To[0].MessageUUID);
        }
    }
}