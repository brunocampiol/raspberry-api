using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using RaspberryPi.API.AutoMapper;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Infrastructure.Models.Emails;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Benchmarks;

[MemoryDiagnoser]
public class AutoMapperBenchmarks
{
    private IMapper _mapper;

    private FactInfraResponse _factInfraResponse;
    private FactViewModel _factViewModel;
    private WeatherDto _weatherDto;
    private EmailViewModel _emailViewModel;
    private EmailDto _emailDto;

    private List<FactInfraResponse> _factInfraResponses;
    private List<FactViewModel> _factViewModels;
    private List<WeatherDto> _weatherDtos;
    private List<EmailViewModel> _emailViewModels;
    private List<EmailDto> _emailDtos;

    //[Params(10, 100, 1000, 10_000, 100_000, 1_000_000)]
    [Params(10)]
    public int CollectionSize;

    [GlobalSetup]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _factInfraResponse = new FactInfraResponse { Text = "Benchmark Fact" };
        _factViewModel = new FactViewModel { Fact = "Benchmark Fact" };
        _weatherDto = new WeatherDto
        {
            EnglishName = "City",
            CountryCode = "CC",
            WeatherText = "Sunny",
            Temperature = "25"
        };
        _emailViewModel = new EmailViewModel
        {
            To = "test@example.com",
            Subject = "Subject",
            Body = "Body",
            IsBodyHtml = true
        };
        _emailDto = new EmailDto
        {
            To = "test2@example.com",
            Subject = "Subject2",
            Body = "Body2",
            IsBodyHtml = false
        };

        _factInfraResponses = new List<FactInfraResponse>(CollectionSize);
        _factViewModels = new List<FactViewModel>(CollectionSize);
        _weatherDtos = new List<WeatherDto>(CollectionSize);
        _emailViewModels = new List<EmailViewModel>(CollectionSize);
        _emailDtos = new List<EmailDto>(CollectionSize);

        for (int i = 0; i < CollectionSize; i++)
        {
            _factInfraResponses.Add(new FactInfraResponse { Text = $"Fact {i}" });
            _factViewModels.Add(new FactViewModel { Fact = $"Fact {i}" });
            _weatherDtos.Add(new WeatherDto
            {
                EnglishName = $"City {i}",
                CountryCode = $"C{i % 100}",
                WeatherText = $"Weather {i}",
                Temperature = (20 + (i % 15)).ToString()
            });
            _emailViewModels.Add(new EmailViewModel
            {
                To = $"user{i}@example.com",
                Subject = $"Subject {i}",
                Body = $"Body {i}",
                IsBodyHtml = i % 2 == 0
            });
            _emailDtos.Add(new EmailDto
            {
                To = $"user{i}@example.com",
                Subject = $"Subject {i}",
                Body = $"Body {i}",
                IsBodyHtml = i % 2 == 1
            });
        }
    }

    // --- Single object mapping benchmarks ---

    [Benchmark]
    public FactViewModel Map_FactInfraResponse_To_FactViewModel() => _mapper.Map<FactViewModel>(_factInfraResponse);

    //[Benchmark]
    //public FactInfraResponse Map_FactViewModel_To_FactInfraResponse() => _mapper.Map<FactInfraResponse>(_factViewModel);

    //[Benchmark]
    //public WeatherViewModel Map_WeatherDto_To_WeatherViewModel() => _mapper.Map<WeatherViewModel>(_weatherDto);

    //[Benchmark]
    //public EmailDto Map_EmailViewModel_To_EmailDto() => _mapper.Map<EmailDto>(_emailViewModel);

    //[Benchmark]
    //public Email Map_EmailDto_To_Email() => _mapper.Map<Email>(_emailDto);

    // --- Collection mapping benchmarks ---

    //[Benchmark]
    //public List<FactViewModel> Map_FactInfraResponseList_To_FactViewModelList() => _mapper.Map<List<FactViewModel>>(_factInfraResponses);

    //[Benchmark]
    //public List<FactInfraResponse> Map_FactViewModelList_To_FactInfraResponseList() => _mapper.Map<List<FactInfraResponse>>(_factViewModels);

    //[Benchmark]
    //public List<WeatherViewModel> Map_WeatherDtoList_To_WeatherViewModelList() => _mapper.Map<List<WeatherViewModel>>(_weatherDtos);

    //[Benchmark]
    //public List<EmailDto> Map_EmailViewModelList_To_EmailDtoList() => _mapper.Map<List<EmailDto>>(_emailViewModels);

    //[Benchmark]
    //public List<Email> Map_EmailDtoList_To_EmailList() => _mapper.Map<List<Email>>(_emailDtos);

    // --- Additional: Mapping to IEnumerable<TDest> (to test interface mapping performance) ---

    [Benchmark]
    public List<FactViewModel> Map_FactInfraResponseList_To_IEnumerableFactViewModel() => _mapper.Map<IEnumerable<FactViewModel>>(_factInfraResponses).ToList();

    //[Benchmark]
    //public List<WeatherViewModel> Map_WeatherDtoList_To_IEnumerableWeatherViewModel() => _mapper.Map<IEnumerable<WeatherViewModel>>(_weatherDtos).ToList();

    //[Benchmark]
    //public List<EmailDto> Map_EmailViewModelList_To_IEnumerableEmailDto() => _mapper.Map<IEnumerable<EmailDto>>(_emailViewModels).ToList();

    //[Benchmark]
    //public List<Email> Map_EmailDtoList_To_IEnumerableEmail() => _mapper.Map<IEnumerable<Email>>(_emailDtos).ToList();
}