namespace RaspberryPi.API.Models;

public class EndpointDetail
{
    private const int MaxRequests = 10_000;
    public long Count { get; private set; }
    public List<RequestDetail> Requests { get; } = [];

    private readonly object _lock = new();

    public void AddRequest(string ipAddress)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ipAddress);

        lock (_lock)
        {
            Count++;
            Requests.Add(new RequestDetail() { IpAddress = ipAddress, TimestampUtc = DateTime.UtcNow });

            if (Requests.Count > MaxRequests)
            {
                Requests.RemoveRange(0, Requests.Count - MaxRequests); // Remove oldest entries
            }
        }
    }
}