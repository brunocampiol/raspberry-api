using RaspberryPi.Domain.Models;

namespace RaspberryPi.Domain.Interfaces.Services;

public interface IGeoLocationProvider
{
    string ProviderName { get; }    // Human-readable identifier
    bool IsAvailable { get; } // Simple health flag (could be updated by a health monitor)
    Task<GeoLocationResult> GetGeoLocationAsync(
        string ipAddress,
        CancellationToken cancellationToken = default);
}