using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RaspberryPi.Domain.Services
{
    public sealed class JwtService(IOptions<JwtOptions> options) : IJwtService
    {
        private readonly JwtOptions _options = options.Value ?? throw new ArgumentNullException(nameof(options));

        public string GenerateTokenForEmail(string email, IEnumerable<string> roles)
        {
            return GenerateToken(null, email, roles);
        }

        public string GenerateTokenForUserName(string userName, IEnumerable<string> roles)
        {
            return GenerateToken(userName, null, roles);
        }

        private string GenerateToken(string userName, string email, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Key);
            var claims = GetClaims(userName, email, roles);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(_options.ExpirationInSeconds),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static List<Claim> GetClaims(string userName, string email, IEnumerable<string> roles)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(userName))
            {
                claims.Add(new Claim(ClaimTypes.Name, userName));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                claims.Add(new Claim(ClaimTypes.Email, email));
            }
     
            // Adds roles
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}