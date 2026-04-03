using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using RaspberryPi.API.Mapping;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Models.Dtos;

namespace RaspberryPi.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class WeatherCollectionBenchmarks
{
    private List<WeatherDto> _collection;

    [Params(10, 100, 1_000, 10_000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        _collection = Enumerable.Range(0, N)
            .Select(i => new WeatherDto
            {
                EnglishName = $"City {i}",
                CountryCode = $"C{i % 100}",
                WeatherText = $"Weather {i}",
                Temperature = (20 + (i % 15)).ToString()
            })
            .ToList();
    }

    [Benchmark(Baseline = true)]
    public List<WeatherViewModel> Baseline()
        => _collection.Select(x => new WeatherViewModel
        {
            EnglishName = x.EnglishName,
            CountryCode = x.CountryCode,
            WeatherText = x.WeatherText,
            Temperature = x.Temperature
        }).ToList();

    [Benchmark]
    public List<WeatherViewModel> Mapper()
        => _collection.Select(x => x.MapToWeatherViewModel()).ToList();
}