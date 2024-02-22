using Microsoft.Extensions.Options;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Options;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Services
{
    public sealed class IdentityAppService : IIdentityAppService
    {
        private readonly IdentityAppOptions _settings;
        private readonly IJwtService _jwtService;

        public IdentityAppService(IOptions<IdentityAppOptions> settings, IJwtService jwtService)
        {
            _settings = settings.Value;
            _jwtService = jwtService;
        }

        public Result<string> Authenticate(string username, string password)
        {
            var user = GetUser(username, password);

            if (user == null)
            {
                return Result<string>.Failure("Invalid user name or password");
            }

            var token = _jwtService.GenerateTokenForUserName(user.UserName, user.Role);

            return Result<string>.Success(token);
        }

        private AspNetUser? GetUser(string email, string password)
        {
            return _settings.AspNetUsers?
                    .FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                                    x.Password.Equals(password, StringComparison.Ordinal));
        }
    }
}