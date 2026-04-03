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
public class WeatherSingleBenchmarks
{
    private WeatherDto _source;

    [GlobalSetup]
    public void Setup()
    {
        _source = new WeatherDto
        {
            EnglishName = "City",
            CountryCode = "CC",
            WeatherText = "Sunny",
            Temperature = "25"
        };
    }

    [Benchmark(Baseline = true)]
    public WeatherViewModel Baseline()
        => new WeatherViewModel
        {
            EnglishName = _source.EnglishName,
            CountryCode = _source.CountryCode,
            WeatherText = _source.WeatherText,
            Temperature = _source.Temperature
        };


    [Benchmark]
    public WeatherViewModel Mapper()
        => _source.MapToWeatherViewModel();
}