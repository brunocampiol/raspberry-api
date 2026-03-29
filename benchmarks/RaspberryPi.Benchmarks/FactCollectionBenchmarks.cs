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
public class FactCollectionBenchmarks
{
    private IMapper _mapper;
    private List<FactInfraResponse> _collection;

    [Params(10, 100, 1_000, 10_000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        config.AssertConfigurationIsValid();

        _mapper = config.CreateMapper();

        _collection = Enumerable.Range(0, N)
            .Select(i => new FactInfraResponse { Text = $"Fact {i}" })
            .ToList();
    }

    [Benchmark(Baseline = true)]
    public List<FactViewModel> Manual()
        => _collection.Select(x => new FactViewModel { Fact = x.Text }).ToList();

    [Benchmark]
    public List<FactViewModel> AutoMapper()
        => _mapper.Map<List<FactViewModel>>(_collection);
}