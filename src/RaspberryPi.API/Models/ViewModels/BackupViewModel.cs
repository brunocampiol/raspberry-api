using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.API.Models.ViewModels
{
    public class BackupViewModel
    {
        public required IEnumerable<Fact> Facts { get; set; }
        public required IEnumerable<GeoLocation> GeoLocations { get; set; }
    }
}