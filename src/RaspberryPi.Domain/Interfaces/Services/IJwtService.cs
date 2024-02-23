namespace RaspberryPi.Domain.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateTokenForUserName(string userName, IEnumerable<string> roles);
        string GenerateTokenForEmail(string email, IEnumerable<string> roles);
    }
}