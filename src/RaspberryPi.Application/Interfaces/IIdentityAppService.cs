using RaspberryPi.Domain.Core;

namespace RaspberryPi.Application.Interfaces
{
    public interface IIdentityAppService
    {
        Result<string> Authenticate(string username, string password);
    }
}