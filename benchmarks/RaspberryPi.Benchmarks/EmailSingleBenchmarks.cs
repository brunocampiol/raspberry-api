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
public class EmailSingleBenchmarks
{
    private IMapper _mapper;
    private EmailDto _source;

    [GlobalSetup]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        config.AssertConfigurationIsValid();

        _mapper = config.CreateMapper();

        _source = new EmailDto
        {
            To = "test@example.com",
            Subject = "Subject",
            Body = "Body",
            IsBodyHtml = true
        };
    }

    [Benchmark(Baseline = true)]
    public Email Manual()
        => new Email
        {
            To = _source.To,
            Subject = _source.Subject,
            Body = _source.Body,
            IsBodyHtml = _source.IsBodyHtml
        };

    [Benchmark]
    public Email AutoMapper()
        => _mapper.Map<Email>(_source);
}