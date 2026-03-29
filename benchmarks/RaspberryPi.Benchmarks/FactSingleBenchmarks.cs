using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using RaspberryPi.API.AutoMapper;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class FactSingleBenchmarks
{
    private IMapper _mapper;
    private FactInfraResponse _source;

    [GlobalSetup]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        config.AssertConfigurationIsValid();

        _mapper = config.CreateMapper();

        _source = new FactInfraResponse { Text = "Benchmark" };
    }

    [Benchmark(Baseline = true)]
    public FactViewModel Manual()
        => new FactViewModel { Fact = _source.Text };

    [Benchmark]
    public FactViewModel AutoMapper()
        => _mapper.Map<FactViewModel>(_source);
}