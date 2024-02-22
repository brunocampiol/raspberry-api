namespace RaspberryPi.Domain.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public ICollection<string> Errors { get; } = new List<string>();

        /// <summary>
        /// Creates a success result with a value
        /// </summary>
        /// <param name="value"></param>
        public Result(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Value = value;
            IsSuccess = true;
        }

        /// <summary>
        /// Creates a failure result with a collection of errors
        /// </summary>
        /// <param name="errors"></param>
        public Result(ICollection<string> errors)
        {
            ArgumentNullException.ThrowIfNull(errors);
            Errors = errors;
        }

        /// <summary>
        /// Creates a failure result with one error
        /// </summary>
        /// <param name="errorMessage"></param>
        public Result(string errorMessage)
        {
            ArgumentException.ThrowIfNullOrEmpty(errorMessage);
            Errors = new List<string>() { errorMessage };
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public static Result<T> Success(T result) => new(result);
        public static Result<T> Failure(string errorMessage) => new(errorMessage);
    }
}