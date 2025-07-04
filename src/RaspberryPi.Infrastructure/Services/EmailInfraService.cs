using Microsoft.Extensions.Options;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Emails;
using RaspberryPi.Infrastructure.Models.Options;
using System.Net;
using System.Net.Mail;

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
        var mail = new MailMessage()
        {
            From = new MailAddress(_settings.FromEmail),
            Subject = email.Subject,
            Body = email.Body,
            IsBodyHtml = email.IsBodyHtml
        };

        mail.To.Add(email.To);

        // TODO: use mailkit instead of System.Net.Mail
        using var smtp = new SmtpClient(_settings.SmtpAddress, _settings.SmtpPort);
        smtp.Credentials = new NetworkCredential(_settings.FromEmail, _settings.SmtpPassword);
        smtp.EnableSsl = true;

        await smtp.SendMailAsync(mail);
    }
}