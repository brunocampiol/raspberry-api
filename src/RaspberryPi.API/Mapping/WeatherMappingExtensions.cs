using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Models.Dtos;

namespace RaspberryPi.API.Mapping;

public static class WeatherMappingExtensions
{
    public static WeatherViewModel MapToWeatherViewModel(this WeatherDto source)
    {
        return new WeatherViewModel
        {
            CountryCode = source.CountryCode,
            EnglishName = source.EnglishName,
            Temperature = source.Temperature,
            WeatherText = source.WeatherText
        };
    }
}