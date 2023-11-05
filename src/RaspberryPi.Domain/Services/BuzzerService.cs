using System.Device.Pwm;

namespace RaspberryPi.Domain.Services
{
    public class BuzzerService : IDisposable
    {
        // https://github.com/dotnet/iot/blob/main/Documentation/raspi-pwm.md
        // https://github.com/gukoff/nanoFramework.IoT.Device/blob/3f41917fe8fe71064ff5a05b0c3a8dd5c0adcf12/src/devices_generated/Buzzer/Buzzer.cs#L13

        private const int _chip = 0;
        private const int _channel = 0;

        private readonly PwmChannel _pwmChannel;
        private bool _disposed = false;

        public BuzzerService()
        {
            // PWM will use GPIO12
            _pwmChannel = PwmChannel.Create(_chip, _channel);
        }

        public void PlayTones()
        {
            _pwmChannel.Start();
            Thread.Sleep(2000);
            _pwmChannel.Frequency = 523;
            Thread.Sleep(2000);
            _pwmChannel.Frequency = 1046;
            Thread.Sleep(2000);
            _pwmChannel.Frequency = 988;
            Thread.Sleep(2000);
            _pwmChannel.Frequency = 740;
            Thread.Sleep(2000);
            _pwmChannel.Frequency = 349;
            Thread.Sleep(2000);
            _pwmChannel.Stop();
        }

        public void PlayTone(int frequency, int milliseconds)
        {
            _pwmChannel.Start();
            _pwmChannel.Frequency = frequency;
            Thread.Sleep(milliseconds);
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
                    // Dispose managed resources here
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