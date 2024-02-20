namespace RaspberryPi.Domain.Interfaces.Services
{
    public interface IBuzzerService : IDisposable
    {
        void PlayTone(int frequency, int milliseconds);
        Task PlayToneAsync(int frequency, int milliseconds);
    }
}