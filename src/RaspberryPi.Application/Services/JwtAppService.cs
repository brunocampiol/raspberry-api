using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RaspberryPi.Application.Services
{
    // TODO move to domain?
    public sealed class JwtAppService : IJwtAppService
    {
        private readonly JwtAppOptions _options;

        public JwtAppService(IOptions<JwtAppOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateToken(string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    //new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddSeconds(_options.ExpirationInSeconds),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}