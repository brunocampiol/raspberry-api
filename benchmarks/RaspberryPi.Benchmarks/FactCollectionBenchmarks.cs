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
public class FactCollectionBenchmarks
{
    private List<FactInfraResponse> _collection = null!;

    [Params(10, 100, 1_000, 10_000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        _collection = Enumerable.Range(0, N)
            .Select(i => new FactInfraResponse { Text = $"Fact {i}" })
            .ToList();
    }

    [Benchmark(Baseline = true)]
    public List<FactViewModel> Baseline()
        => _collection.Select(x => new FactViewModel { Fact = x.Text }).ToList();

    [Benchmark]
    public List<FactViewModel> AutoMapper()
        => _collection.Select(x => x.MapToFactViewModel()).ToList();
}