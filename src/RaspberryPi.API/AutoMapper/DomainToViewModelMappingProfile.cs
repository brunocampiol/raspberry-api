using AutoMapper;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.API.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<FactInfraDto, FactViewModel>();
            CreateMap<WeatherDto, WeatherViewModel>();
        }
    }
}