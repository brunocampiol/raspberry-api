using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Application.Interfaces
{
    public interface IGeoLocationAppService
    {
        Task<LookUpInfraDto> LookUpAsync(string ipAddress);

        Task<LookUpInfraDto> LookUpFromRandomIpAddressAsync();
    }
}