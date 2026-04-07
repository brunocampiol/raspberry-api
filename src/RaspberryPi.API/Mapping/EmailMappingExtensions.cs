using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Models.Dtos;

namespace RaspberryPi.API.Mapping;

public static class EmailMappingExtensions
{
    public static EmailDto MapToEmailDto(this EmailViewModel source)
    {
        return new EmailDto
        {
            To = source.To,
            Subject = source.Subject,
            Body = source.Body,
            IsBodyHtml = source.IsBodyHtml
        };
    }
}