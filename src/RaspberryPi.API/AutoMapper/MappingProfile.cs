using AutoMapper;
using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Application.Models.Dtos;
using RaspberryPi.Infrastructure.Models.Emails;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FactInfraResponse, FactViewModel>()
                .ForMember(
                    dest => dest.Fact,
                    options => options.MapFrom(src => src.Text))
                .ReverseMap();

            CreateMap<WeatherDto, WeatherViewModel>();

            CreateMap<EmailViewModel, EmailDto>();
            CreateMap<EmailDto, Email>();

            //.ForMember(dest => dest.Body, options => options.MapFrom(src => src.Body));
        }
    }
}