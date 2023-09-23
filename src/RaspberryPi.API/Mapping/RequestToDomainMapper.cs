using RaspberryPi.API.Models.Data;
using RaspberryPi.API.Models.Requests;
using Riok.Mapperly.Abstractions;

namespace RaspberryPi.API.Mapping
{
    [Mapper]
    public partial class RequestToDomainMapper : IRequestToDomainMapper
    {
        public partial AspNetUser CreateAspNetUserRequestToAspNetUser(CreateAspNetUserRequest request);

        //public AspNetUser CreateAspNetUserRequestToAspNetUser(CreateAspNetUserRequest request)
        //{
        //    var user = MapCreateAspNetUserRequestToAspNetUser(request);
        //    user.Role = "user";
        //    user.DateCreateUTC = DateTime.UtcNow;

        //    return user;
        //}

        //public partial AspNetUser MapCreateAspNetUserRequestToAspNetUser(CreateAspNetUserRequest request);
    }
}