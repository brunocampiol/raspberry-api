using RaspberryPi.Domain.Interfaces.Services;
using System.Device.Pwm;

namespace RaspberryPi.Domain.Services
{
    public class BuzzerService : IBuzzerService
    {
        // https://github.com/dotnet/iot/blob/main/Documentation/raspi-pwm.md
        // https://github.com/gukoff/nanoFramework.IoT.Device/blob/3f41917fe8fe71064ff5a05b0c3a8dd5c0adcf12/src/devices_generated/Buzzer/Buzzer.cs#L13

        private const int _chip = 0;
        private const int _channel = 0;

        private readonly PwmChannel _pwmChannel;
        private bool _disposed;

        public BuzzerService()
        {
            // PWM will use GPIO12
            _pwmChannel = PwmChannel.Create(_chip, _channel);
        }

        public void PlayTone(int frequency, int milliseconds)
        {
            _pwmChannel.Start();
            _pwmChannel.Frequency = frequency;
            Thread.Sleep(milliseconds);
            _pwmChannel.Stop();
        }

        public async Task PlayToneAsync(int frequency, int milliseconds)
        {
            _pwmChannel.Start();
            _pwmChannel.Frequency = frequency;
            await Task.Delay(milliseconds);
            _pwmChannel.Stop();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _pwmChannel.Stop();
                    _pwmChannel.Dispose();
                }

                _disposed = true;
            }
        }

        ~BuzzerService()
        {
            Dispose(false);
        }
    }
}