using AutoMapper;
using RaspberryPi.Application.Models.ViewModels;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.Application.AutoMapper
{
    public class InfraToAppModelMappingProfile : Profile
    {
        public InfraToAppModelMappingProfile()
        {
            //CreateMap<FactResponse, FactViewModel>();
        }
    }
}
