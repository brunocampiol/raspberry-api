using RaspberryPi.Domain.Models;

namespace RaspberryPi.Application.Interfaces
{
    public interface IMusicAppService : IDisposable
    {
        void PlayMusic(Music music);
        void PlayNokiaRingTone();
    }
}