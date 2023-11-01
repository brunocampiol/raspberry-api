using RaspberryPi.Infrastructure.Models.IpGeolocation;

namespace RaspberryPi.Infrastructure.Interfaces
{
    public interface IApiIpService
    {
        Task<ApiIpCheck> Check(string ipAddress);
    }
}