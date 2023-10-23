using Fetchgoods.Text.Json.Extensions;
using RaspberryPi.Application.Interfaces;
using System.Device.Gpio;

namespace RaspberryPi.Application.Services
{
    public class GpioAppService : IGpioAppService
    {
        // Refers to Raspberry Pi 2 model b+
        // https://pinout.xyz/

        // https://learn.microsoft.com/pt-br/dotnet/iot/

        // https://github.com/andycb/PiSharp
        // https://github.com/Ramon-Balaguer/raspberry-sharp-io
        // https://github.com/AlexSartori/Raspberry-GPIO-Manager

        public GpioAppService()
        {

        }

        // GPIO 26
        public string ReadGpio26()
        {
            const int pin = 26; // GPIO26 or 37 physical/board
            using var controller = new GpioController();
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
