namespace RaspberryPi.Domain.Interfaces.Services
{
    public interface IBuzzerService
    {
        void Dispose();
        void PlayTone(int frequency, int milliseconds);
        Task PlayToneAsync(int frequency, int milliseconds);
        void PlayTones();
        Task PlayTonesAsync();
    }
}