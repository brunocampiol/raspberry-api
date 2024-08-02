using RaspberryPi.Domain.Models;

namespace RaspberryPi.Application.Models.Options
{
    public sealed class IdentityAppOptions
    {
        public const string SectionName = "IdentityAppOptions";
        public ICollection<AspNetUser>? AspNetUsers { get; init; }
    }
}