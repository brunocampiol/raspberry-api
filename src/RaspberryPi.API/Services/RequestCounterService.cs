using RaspberryPi.API.Models;
using System.Collections.Concurrent;

namespace RaspberryPi.API.Services;

public class RequestCounterService
{
    private readonly ConcurrentDictionary<string, EndpointDetail> _stats = new();

    public void Increment(string controller, string action, string ipAddress)
    {
        var key = $"{controller}.{action}";
        _stats.AddOrUpdate(
            key,
            _ =>
            {
                var stats = new EndpointDetail();
                stats.AddRequest(ipAddress);
                return stats;
            },
            (_, stats) =>
            {
                stats.AddRequest(ipAddress);
                return stats;
            });
    }

    public IReadOnlyDictionary<string, EndpointDetail> GetAll() => _stats;
}