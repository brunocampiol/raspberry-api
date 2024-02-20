using RaspberryPi.Domain.Models;

namespace RaspberryPi.Application.Interfaces
{
    public interface IMusicAppService
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