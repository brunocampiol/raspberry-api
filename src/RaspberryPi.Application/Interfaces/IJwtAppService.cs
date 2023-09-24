namespace RaspberryPi.Application.Interfaces
{
    public interface IJwtAppService
    {
        string GenerateToken(string email, string role);
    }
}