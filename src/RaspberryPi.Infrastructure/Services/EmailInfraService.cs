using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Emails;
using RaspberryPi.Infrastructure.Models.Options;

namespace RaspberryPi.Infrastructure.Services;

public sealed class EmailInfraService : IEmailInfraService
{
    private readonly EmailOptions _settings;

    public EmailInfraService(IOptions<EmailOptions> settings)
    {
        ArgumentNullException.ThrowIfNull(settings.Value);
        _settings = settings.Value;
    }

    public async Task SendEmailAsync(Email email)
    {
        ArgumentNullException.ThrowIfNull(email);
        if (string.IsNullOrWhiteSpace(email.To))
        {
            var errorMessage = "To email address is required and " +
                               "cannot be null, empty or consists " +
                               "only of white-space characters.";
            throw new ArgumentException(errorMessage);
        }

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_settings.SmtpAddress, _settings.SmtpPort)
                      .ConfigureAwait(false);
            await smtp.AuthenticateAsync(_settings.FromEmail, _settings.SmtpPassword)
                      .ConfigureAwait(false);

            var message = CreateMimeMessage(email);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        finally
        {
            await smtp.DisconnectAsync(true)
                      .ConfigureAwait(false);
        }
    }

    private MimeMessage CreateMimeMessage(Email email)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        message.To.Add(new MailboxAddress(email.To, email.To));
        message.Subject = email.Subject;

        if (email.IsBodyHtml)
        {
            message.Body = new TextPart(TextFormat.Html) { Text = email.Body };
        }
        else
        {
            message.Body = new TextPart(TextFormat.Plain) { Text = email.Body };
        }

        return message;
    }
}