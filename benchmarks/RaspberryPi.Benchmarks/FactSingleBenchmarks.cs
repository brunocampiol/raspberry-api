using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using RaspberryPi.API.Mapping;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class FactSingleBenchmarks
{
    private FactInfraResponse _source;

    [GlobalSetup]
    public void Setup()
    {
        _source = new FactInfraResponse { Text = "Benchmark" };
    }

    [Benchmark(Baseline = true)]
    public FactViewModel Baseline()
        => new FactViewModel { Fact = _source.Text };

    [Benchmark]
    public FactViewModel AutoMapper()
        => _source.MapToFactViewModel();
}