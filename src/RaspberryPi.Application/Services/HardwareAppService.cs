using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Extensions;
using System.Device.Gpio;

namespace RaspberryPi.Application.Services
{
    public class HardwareAppService : IHardwareAppService
    {
        // Refers to Raspberry Pi 2 model b+
        // https://pinout.xyz/

        // https://learn.microsoft.com/en-us/dotnet/iot/

        // https://github.com/andycb/PiSharp
        // https://github.com/Ramon-Balaguer/raspberry-sharp-io
        // https://github.com/AlexSartori/Raspberry-GPIO-Manager

        public HardwareAppService()
        {

        }

        public void BlinkLedGpio26()
        {
            const int pin = 26; // GPIO26 or 37 physical/board
            using var controller = new GpioController();
            controller.OpenPin(pin, PinMode.Output);

            for (int i = 0; i < 40; i++)
            {
                controller.Write(pin, PinValue.High);
                Thread.Sleep(250);
                controller.Write(pin, PinValue.Low);
                Thread.Sleep(250);
            }            
        }

        // GPIO 26
        public string ReadGpio26()
        {
            const int pin = 26; // GPIO26 or 37 physical/board
            using var controller = new GpioController();
            controller.OpenPin(pin, PinMode.Input);
            var readValue = controller.Read(pin);
            return readValue.ToJson();
        }

        //public string ReadAllGpio()
        //{
        //    // create a code that will return all gpio pins from a raspberry pi 2 model b+ as a string that resembles the actual hardware pinout.
        //    using var controller = new GpioController();
        //    controller.OpenPin(Pin, PinMode.InputPullUp);
        //}

    }
}
