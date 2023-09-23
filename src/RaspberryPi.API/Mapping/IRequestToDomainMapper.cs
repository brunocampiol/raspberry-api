using RaspberryPi.API.Models.Data;
using RaspberryPi.API.Models.Requests;

namespace RaspberryPi.API.Mapping
{
    public interface IRequestToDomainMapper
    {
        AspNetUser CreateAspNetUserRequestToAspNetUser(CreateAspNetUserRequest request);
    }
}