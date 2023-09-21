using RaspberryPi.API.Models.Data;

namespace RaspberryPi.API.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}