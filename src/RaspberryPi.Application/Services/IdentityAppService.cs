using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Services
{
    public sealed class IdentityAppService : IIdentityAppService
    {
        private readonly IJwtService _jwtService;

        public IdentityAppService(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public OperationResult<string> Authenticate(string username, string password)
        {
            var user = GetUser(username, password);

            if (user == null)
            {
                return OperationResult<string>.Failure("Invalid user name or password");
            }

            var token = _jwtService.GenerateTokenForUserName(user.UserName, user.Role);

            return OperationResult<string>.Success(token);
        }

        private AspNetUser? GetUser(string email, string password)
        {
            var users = new List<AspNetUser>()
            {
                new AspNetUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "root",
                    Email = "root",
                    Password = "root",
                    Role = "root"
                },
                new AspNetUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "user",
                    Email = "user",
                    Password = "user",
                    Role = "user"
                }
            };

            return users.Find(x => x.Email.ToUpperInvariant() == email.ToUpperInvariant() && x.Password == password);
        }
    }
}
