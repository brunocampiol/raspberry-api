using System.Runtime.Serialization;

namespace RaspberryPi.Domain.Core
{
    public class AppException : ApplicationException
    {
        public AppException()
        {
        }

        public AppException(string message)
            : base(message)
        {
        }

        public AppException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected AppException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}