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
            using var pwm = PwmChannel.Create(_chip, _channel, 400, 0.5);
            pwm.Start();
            Thread.Sleep(2000);
            pwm.Stop();
        }

        public void PlayStarWarsTheme()
        {
            // TODO: fix index outside bounds exception
            const int duration = 500; // Set the duration of each note in milliseconds

            int[] melody = {
                440, 440, 440, 349, 523, 440, 349, 523, 440, 0, 659, 659, 659, 698, 523, 415,
                349, 523, 440, 0, 880, 440, 880, 349, 1760, 440, 349, 523, 880, 784, 349, 740,
                698, 523, 523, 988, 659, 415, 349, 523, 440, 587, 523, 440, 349, 698, 523, 349,
                523, 440, 0, 880, 440, 1760, 349, 523, 523, 1046, 988, 349, 740, 698, 523, 523,
                988, 659, 415, 349, 523, 440, 587, 523, 440, 349, 698, 523, 349, 523, 440, 0,
                659, 784, 880, 698, 349, 523, 349, 440, 523, 349, 784, 659, 440, 349, 523, 440
            };

            int[] noteDurations = {
                500, 500, 500, 350, 150, 500, 350, 150, 1000, 500, 500, 500, 500, 350, 150, 500,
                350, 150, 1000, 500, 500, 350, 150, 500, 350, 150, 1000, 500, 350, 150, 1000, 500,
                500, 500, 350, 150, 500, 350, 150, 1000, 1000, 1000, 500, 500, 500, 500, 350, 150,
                500, 350, 150, 1000, 500, 500, 350, 150, 500, 350, 150, 1000, 500, 350, 150, 1000
            };

            using PwmChannel pwmChannel = PwmChannel.Create(_chip, _channel);
            pwmChannel.Start();

            for (int i = 0; i < melody.Length; i++)
            {
                int noteDuration = duration / noteDurations[i];
                pwmChannel.Frequency = melody[i];
                Thread.Sleep(noteDuration);
            }

            pwmChannel.Stop();
        }

    }
}