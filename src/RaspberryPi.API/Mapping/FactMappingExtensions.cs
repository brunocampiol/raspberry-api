using RaspberryPi.API.Models.ViewModels;
using RaspberryPi.Infrastructure.Models.Facts;

namespace RaspberryPi.API.Mapping;

public static class FactMappingExtensions
{
    public static FactViewModel MapToFactViewModel(this FactInfraResponse source)
    {
        return new FactViewModel
        {
            Fact = source.Text
        };
    }

    public static FactInfraResponse MapToFactInfraResponse(this FactViewModel source)
    {
        return new FactInfraResponse
        {
            Text = source.Fact
        };
    }
}