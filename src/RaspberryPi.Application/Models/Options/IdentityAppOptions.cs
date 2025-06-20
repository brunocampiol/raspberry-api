using RaspberryPi.Domain.Models;

namespace RaspberryPi.Application.Models.Options;

public record IdentityAppOptions
{
    public const string SectionName = nameof(IdentityAppOptions);
    public ICollection<AspNetUser>? AspNetUsers { get; init; }
}