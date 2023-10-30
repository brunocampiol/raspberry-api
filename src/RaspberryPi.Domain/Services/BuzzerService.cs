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
            PlayMusic(StarWarsTheme());
        }

        public void PlaySuperMarioWorld()
        {
            PlayMusic(SuperMarioWorld());
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

        // TODO needs to fix the sound quality
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

        // TODO needs to fix the sound quality
        static Music SuperMarioWorld()
        {
            int[] marioMelody = new int[] {
                // Super Mario World - Main Theme Melody
                55, 55, 55, 51, 58, 55, 51, 63,
                66, 63, 58, 55, 51, 55, 63, 63,
                66, 55, 55, 61, 61, 58, 58,
                55, 55, 51, 58, 55, 51, 63,
                66, 63, 58, 55, 51, 55, 63, 63,
                66, 70, 70, 63, 63, 61, 61,
                63, 58, 58, 55, 55, 58, 58,
                63, 63, 66, 63, 58, 55, 51, 55, 63, 63,
                66, 70, 70, 63, 63, 61, 61,
                63, 58, 58, 55, 55, 58, 58,
                63, 63, 66, 70, 70, 66, 63, 61,
                58, 55, 61, 61, 63, 58, 55, 51,
                55, 63, 63, 66, 70, 70, 66, 63,
                61, 58, 55, 61, 61, 63, 58, 55, 51
            };

            int[] noteDurations = new int[] {
                // Durations for the Super Mario World - Main Theme Melody
                500, 500, 500, 500, 500, 500, 1000, 500,
                500, 500, 500, 500, 500, 500, 1000, 500,
                500, 500, 500, 500, 500, 500, 500,
                500, 500, 500, 500, 500, 500, 1000, 500,
                500, 500, 500, 500, 500, 500, 500, 500,
                500, 500, 500, 500, 500, 500, 500,
                500, 500, 500, 500, 500, 500, 500,
                500, 500, 500, 500, 500, 500, 1000, 500, 500, 500,
                500, 500, 500, 500, 500, 500, 500,
                500, 500, 500, 500, 500, 500, 500,
                500, 500, 500, 500, 500, 500, 500, 500,
                500, 500, 500, 500, 500, 500, 500, 500,
                500, 500, 500, 500, 500, 500, 1000, 500, 500, 500,
                500, 500, 500, 500, 500, 500
            };

            return new Music(marioMelody, noteDurations);
        }
    }
}