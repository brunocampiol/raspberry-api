namespace RaspberryPi.Domain.Core
{
    public class OperationResult<T>
    {
        public T? Value { get; }
        public bool IsSuccess { get; }
        public ICollection<string> Errors { get; } = new List<string>();

        /// <summary>
        /// Creates a success operation result with a value
        /// </summary>
        /// <param name="value"></param>
        public OperationResult(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Value = value;
            IsSuccess = true;
        }

        /// <summary>
        /// Creates a failure operation result with a collection of errors
        /// </summary>
        /// <param name="errors"></param>
        public OperationResult(ICollection<string> errors)
        {
            ArgumentNullException.ThrowIfNull(errors);
            Errors = errors;
        }

        /// <summary>
        /// Creates a failure operation result with one error
        /// </summary>
        /// <param name="errorMessage"></param>
        public OperationResult(string errorMessage)
        {
            ArgumentException.ThrowIfNullOrEmpty(errorMessage);
            Errors = new List<string>() { errorMessage };
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public static OperationResult<T> Success(T result) => new(result);
        public static OperationResult<T> Failure(string errorMessage) => new(errorMessage);
    }
}