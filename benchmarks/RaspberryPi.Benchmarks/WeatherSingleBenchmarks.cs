using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using RaspberryPi.API.AutoMapper;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Models.Dtos;

namespace RaspberryPi.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class WeatherSingleBenchmarks
{
    private IMapper _mapper;
    private WeatherDto _source;

    [GlobalSetup]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        config.AssertConfigurationIsValid();

        _mapper = config.CreateMapper();

        _source = new WeatherDto
        {
            EnglishName = "City",
            CountryCode = "CC",
            WeatherText = "Sunny",
            Temperature = "25"
        };
    }

    [Benchmark(Baseline = true)]
    public WeatherViewModel Manual()
        => new WeatherViewModel
        {
            EnglishName = _source.EnglishName,
            CountryCode = _source.CountryCode,
            WeatherText = _source.WeatherText,
            Temperature = _source.Temperature
        };

    [Benchmark]
    public WeatherViewModel AutoMapper()
        => _mapper.Map<WeatherViewModel>(_source);
}