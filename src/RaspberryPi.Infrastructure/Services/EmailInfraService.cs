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

    public async Task SendEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        if (string.IsNullOrWhiteSpace(email.To))
        {
            var errorMessage = "Destination email address is required and " +
                               "cannot be null, empty or consists " +
                               "only of white-space characters.";
            throw new ArgumentException(errorMessage);
        }

        using var smtp = new SmtpClient();
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(20));

        try
        {
            await smtp.ConnectAsync(_settings.SmtpAddress, _settings.SmtpPort, true, cts.Token);
            await smtp.AuthenticateAsync(_settings.FromEmail, _settings.SmtpPassword, cts.Token);

            var message = CreateMimeMessage(email);

            await smtp.SendAsync(message, cts.Token);
        }
        finally
        {
            if (smtp.IsConnected)
            {
                await smtp.DisconnectAsync(true);
            }
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