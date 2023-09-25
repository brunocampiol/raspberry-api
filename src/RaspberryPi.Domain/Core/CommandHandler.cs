using FluentValidation.Results;
using RaspberryPi.Domain.Data;

namespace RaspberryPi.Domain.Core
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string mensage)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensage));
        }

        protected ValidationResult Commit(IUnitOfWork uow, string message)
        {
            if (!uow.Commit()) AddError(message);

            return ValidationResult;
        }

        protected ValidationResult Commit(IUnitOfWork uow)
        {
            return Commit(uow, "There was an error saving data");
        }

        protected async Task<ValidationResult> CommitAsync(IUnitOfWork uow, string message)
        {
            if (! await uow.CommitAsync()) AddError(message);

            return ValidationResult;
        }

        protected async Task<ValidationResult> CommitAsync(IUnitOfWork uow)
        {
            return await CommitAsync(uow, "There was an error saving data").ConfigureAwait(false);
        }
    }
}