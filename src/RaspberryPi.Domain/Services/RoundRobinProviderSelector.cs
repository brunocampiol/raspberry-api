using RaspberryPi.Domain.Interfaces.Services;
using System.Collections.Immutable;

namespace RaspberryPi.Domain.Services;

public sealed class RoundRobinProviderSelector : IGeoLocationProviderSelector
{
    private readonly ImmutableArray<IGeoLocationProvider> _providers;
    private int _nextIndex = -1;

    public RoundRobinProviderSelector(IEnumerable<IGeoLocationProvider> providers)
    {
        _providers = providers.ToImmutableArray();

        if (_providers.IsDefaultOrEmpty)
            throw new InvalidOperationException("At least one provider must be registered.");
    }

    public IGeoLocationProvider? GetNextAvailableProvider()
    {
        var count = _providers.Length;

        // Each caller starts at a different position.
        var start = (int)((uint)Interlocked.Increment(ref _nextIndex) % (uint)count);

        for (var i = 0; i < count; i++)
        {
            var provider = _providers[(start + i) % count];
            if (provider.IsAvailable) return provider;
        }

        return null;
    }
}