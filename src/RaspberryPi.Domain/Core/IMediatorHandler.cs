using FluentValidation.Results;

namespace RaspberryPi.Domain.Core
{
    public interface IMediatorHandler
    {
        //Task PublishEvent<T>(T @event) where T : Event;
        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
    }
}