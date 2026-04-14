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
public class EmailSingleBenchmarks
{
    private EmailDto _source = null!;

    [GlobalSetup]
    public void Setup()
    {
        _source = new EmailDto
        {
            To = "test@example.com",
            Subject = "Subject",
            Body = "Body",
            IsBodyHtml = true
        };
    }

    [Benchmark(Baseline = true)]
    public Email Baseline()
        => new Email
        {
            To = _source.To,
            Subject = _source.Subject,
            Body = _source.Body,
            IsBodyHtml = _source.IsBodyHtml
        };

    [Benchmark]
    public Email Mapper()
        => _source.MapToEmail();
}