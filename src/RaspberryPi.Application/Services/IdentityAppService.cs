using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Models.Entity;

namespace RaspberryPi.Application.Services
{
    public sealed class IdentityAppService : IIdentityAppService
    {
        private readonly IJwtAppService _jwtAppService;

        public IdentityAppService(IJwtAppService jwtAppService)
        {
            _jwtAppService = jwtAppService;
        }

        public OperationResult<string> Authenticate(string username, string password)
        {
            var user = GetUser(username, password);

            if (user == null)
            {
                return OperationResult<string>.Failure("Invalid user name or password");
            }

            var token = _jwtAppService.GenerateToken(user.Email, user.Role);

            return OperationResult<string>.Success(token);
        }

        private AspNetUser? GetUser(string email, string password)
        {
            var users = new List<AspNetUser>()
            {
                new AspNetUser
                {
                    Id = Guid.NewGuid(),
                    Email = "root",
                    Password = "root",
                    Role = "root"
                },
                new AspNetUser
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Password = "admin",
                    Role = "admin"
                }
            };

            return users.Find(x => x.Email.ToUpperInvariant() == email.ToUpperInvariant() && x.Password == password);
        }
    }
}
