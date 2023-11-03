using RaspberryPi.Infrastructure.Models.IpGeolocation;

namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IApiIPService
    {
        Task<ApiIpCheck> Check(string ipAddress);
    }
}