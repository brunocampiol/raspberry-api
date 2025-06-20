using System.Collections.Concurrent;

namespace RaspberryPi.API.Services;

public class RequestCounterService
{
    private readonly ConcurrentDictionary<string, long> _counts = new();

    public void Increment(string controller, string action)
    {
        var key = $"{controller}.{action}";
        _counts.AddOrUpdate(key, 1, (_, count) => count + 1);
    }

    public IReadOnlyDictionary<string, long> GetAll() => _counts;
}