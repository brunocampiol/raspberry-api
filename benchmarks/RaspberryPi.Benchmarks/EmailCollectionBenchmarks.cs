using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using RaspberryPi.Application.Mapping;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Infrastructure.Models.Emails;

namespace RaspberryPi.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class EmailCollectionBenchmarks
{
    private List<EmailDto> _collection;

    [Params(10, 100, 1_000, 10_000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        _collection = Enumerable.Range(0, N)
            .Select(i => new EmailDto
            {
                To = $"user{i}@example.com",
                Subject = $"Subject {i}",
                Body = $"Body {i}",
                IsBodyHtml = i % 2 == 0
            })
            .ToList();
    }

    [Benchmark(Baseline = true)]
    public List<Email> Baseline()
        => _collection.Select(x => new Email
        {
            To = x.To,
            Subject = x.Subject,
            Body = x.Body,
            IsBodyHtml = x.IsBodyHtml
        }).ToList();

    [Benchmark]
    public List<Email> Mapper()
        => _collection.Select(x => x.MapToEmail()).ToList();
}