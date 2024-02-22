using RaspberryPi.Domain.Core;

namespace RaspberryPi.Application.Interfaces
{
    public interface IIdentityAppService
    {
        OperationResult<string> Authenticate(string username, string password);
    }
}