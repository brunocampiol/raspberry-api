namespace RaspberryPi.Domain.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateTokenForUserName(string userName, string role);
        string GenerateTokenForEmail(string email, string role);
    }
}