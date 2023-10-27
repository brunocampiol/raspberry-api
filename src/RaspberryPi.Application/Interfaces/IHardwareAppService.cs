namespace RaspberryPi.Application.Interfaces
{
    public interface IHardwareAppService
    {
        void BlinkLedGpio26();
        string ReadGpio26();
    }
}