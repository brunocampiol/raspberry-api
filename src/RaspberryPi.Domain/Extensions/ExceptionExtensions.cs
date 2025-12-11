using System.Text;

namespace RaspberryPi.Domain.Extensions;

public static class ExceptionExtensions
{
    /// <summary>
    /// Recursively collects all exception messages, including inner and aggregate exceptions.
    /// </summary>
    /// <param name="ex">The exception to extract messages from.</param>
    /// <returns>A concatenated string of all messages.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="ex"/> is null.</exception>
    public static string AllMessages(this Exception ex)
    {
        ArgumentNullException.ThrowIfNull(ex);

        var sb = new StringBuilder();
        sb.Append(ex.Message);

        switch (ex)
        {
            case AggregateException:
                // Aggregate exceptions Message property produces a computed
                // string that describes the number of inner exceptions
                break;
            default:
                if (ex.InnerException != null)
                {
                    sb.Append(" --> ").Append(ex.InnerException.AllMessages()); // recursion here
                }
                break;
        }

        return sb.ToString();
    }
}