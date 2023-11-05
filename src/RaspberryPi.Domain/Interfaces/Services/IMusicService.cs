using RaspberryPi.Domain.Models;

namespace RaspberryPi.Domain.Interfaces.Services
{
    public interface IMusicService
    {
        void Dispose();
        void PlayImperialMarch();
        void PlayMusic(Music music);
        void PlayNokiaRingtone();
        void PlayPinkPanther();
        void PlayPiratesOfTheCaribbean();
        void PlaySuperMarioWorld();
    }
}