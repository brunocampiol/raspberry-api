using AutoMapper;
using RaspberryPi.API.Models;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.API.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<FactInfraDto, FactViewModel>();
        }
    }
}