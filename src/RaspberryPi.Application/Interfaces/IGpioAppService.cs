namespace RaspberryPi.Application.Interfaces
{
    public interface IGpioAppService
    {
        string ReadGpio26();
        void TogglePin18();
    }
}