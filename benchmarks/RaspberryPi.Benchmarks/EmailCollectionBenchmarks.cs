using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using RaspberryPi.API.AutoMapper;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Infrastructure.Models.Emails;

namespace RaspberryPi.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class EmailCollectionBenchmarks
{
    private IMapper _mapper;
    private List<EmailDto> _collection;

    [Params(10, 100, 1_000, 10_000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        config.AssertConfigurationIsValid();

        _mapper = config.CreateMapper();

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
    public List<Email> Manual()
        => _collection.Select(x => new Email
        {
            To = x.To,
            Subject = x.Subject,
            Body = x.Body,
            IsBodyHtml = x.IsBodyHtml
        }).ToList();

    [Benchmark]
    public List<Email> AutoMapper()
        => _mapper.Map<List<Email>>(_collection);
}