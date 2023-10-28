using RaspberryPi.Domain.Models;
using System.Device.Pwm;

namespace RaspberryPi.Domain.Services
{
    public class BuzzerService
    {
        // https://github.com/dotnet/iot/blob/main/Documentation/raspi-pwm.md
        // https://github.com/gukoff/nanoFramework.IoT.Device/blob/3f41917fe8fe71064ff5a05b0c3a8dd5c0adcf12/src/devices_generated/Buzzer/Buzzer.cs#L13

        private const int _chip = 0;
        private const int _channel = 0;

        public BuzzerService()
        {
        }

        public void Play()
        {
            // PWM will use GPIO12
            using var pwm = PwmChannel.Create(_chip, _channel);
            pwm.Start();
            Thread.Sleep(2000);
            pwm.Stop();
        }

        public void PlayTones()
        {
            // PWM will use GPIO12
            using var pwm = PwmChannel.Create(_chip, _channel);
            pwm.Start();
            Thread.Sleep(2000);
            pwm.Frequency = 523;
            Thread.Sleep(2000);
            pwm.Frequency = 1046;
            Thread.Sleep(2000);
            pwm.Frequency = 988;
            Thread.Sleep(2000);
            pwm.Frequency = 740;
            Thread.Sleep(2000);
            pwm.Frequency = 349;
            Thread.Sleep(2000);
            pwm.Stop();
        }

        public void PlayStarWarsTheme()
        {
            var starWarsMusic = StarWarsTheme();
            PlayMusic(starWarsMusic);
        }

        public void PlayMusic(Music music)
        {
            using var pwm = PwmChannel.Create(_chip, _channel);
            pwm.Start();
            for (int i = 0; i < music.Melody.Length; i++)
            {
                pwm.Frequency = music.Melody[i];
                Thread.Sleep(music.NoteDurations[i]);
            }
            pwm.Stop();
        }


        static Music StarWarsTheme()
        {
            int[] starWarsMelody = new int[] {
                // Star Wars Main Theme
                392, 392, 392, 311, 466, 392,
                587, 587, 587, 622, 466, 370,
                392, 392, 392, 311, 466, 392,
                587, 622, 784, 740, 622,
                195, 294, 349, 330,
                311, 466, 392, 311, 466, 587,
                784, 740, 622, 195, 294, 349, 330,
                311, 466, 392, 311, 466, 587,
                784, 740, 622, 784, 1046, 988, 784,
                392, 392, 392, 311, 466, 392,
                587, 622, 784, 740, 622,
                195, 294, 349, 330,
                622, 466, 740, 622,
                622, 466, 740, 622, 784, 1046, 988, 784,
                392, 392, 392, 311, 466, 392
            };

                        int[] starWarsNoteDurations = new int[] {
                // Note Durations for Star Wars Theme
                500, 500, 500, 350, 150, 500,
                350, 150, 500, 500, 350, 150,
                500, 500, 500, 350, 150, 500,
                350, 150, 1000, 500, 350,
                150, 1000, 500, 500, 350, 500,
                500, 350, 150, 500, 350, 150,
                500, 500, 350, 150, 500, 500,
                350, 150, 1000, 500, 350,
                150, 1000, 500, 350, 150, 500,
                500, 500, 500, 350, 150, 500,
                350, 150, 500, 500, 350, 150,
                500, 500, 500, 350, 150, 500,
                350, 150, 1000, 500, 350, 150,
                500, 500, 350, 150, 500, 0, 0,
                0, 0, 0
            };

            var starWarsMusic = new Music(starWarsMelody, starWarsNoteDurations);
            return starWarsMusic;
        }
    }
}