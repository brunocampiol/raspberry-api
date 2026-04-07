using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Infrastructure.Models.Emails;

namespace RaspberryPi.Application.Mapping;

public static class EmailMappingExtensions
{
    public static Email MapToEmail(this EmailDto source)
    {
        return new Email
        {
            To = source.To,
            Subject = source.Subject,
            Body = source.Body,
            IsBodyHtml = source.IsBodyHtml
        };
    }
}