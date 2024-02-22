using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Models.Options
{
    public sealed class IdentityAppOptions
    {
        public const string SectionName = "IdentityAppOptions";
        public ICollection<AspNetUser>? AspNetUsers { get; init; }
    }
}