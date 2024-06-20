using AutoMapper;
using RaspberryPi.Application.Models.ViewModels;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.AutoMapper
{
    public class DomainToAppModelMappingProfile : Profile
    {
        public DomainToAppModelMappingProfile()
        {
            //CreateMap<Fact, FactViewModel>();
        }
    }
}