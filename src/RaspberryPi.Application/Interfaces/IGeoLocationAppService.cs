using RaspberryPi.Infrastructure.Models.GeoLocation;

namespace RaspberryPi.Application.Interfaces
{
    public interface IGeoLocationAppService
    {
        Task<LookUpResponse> LookUpAsync(string ipAddress);

        Task<LookUpResponse> LookUpFromRandomIpAddressAsync();
    }
}