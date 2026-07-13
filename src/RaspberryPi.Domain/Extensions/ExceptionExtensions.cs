using System.Text;

namespace RaspberryPi.Domain.Extensions;

public static class ExceptionExtensions
{
    /// <summary>
    /// Recursively collects all exception messages.
    /// </summary>
    /// <param name="ex"></param>
    /// <returns>A concatenated string of all messages.</returns>
    public static string AllMessages(this Exception ex)
    {
        ArgumentNullException.ThrowIfNull(ex);
        var sb = new StringBuilder();
        switch (ex)
        {
            case AggregateException aggEx:
                // AggregateException.Message auto-appends inner exception messages (non-recursively),
                // so we extract just the headline and recurse into each inner exception ourselves.
                sb.Append(GetAggregateHeadline(aggEx));
                foreach (var innerEx in aggEx.InnerExceptions)
                {
                    sb.Append(" (").Append(innerEx.AllMessages()).Append(')');
                }
                break;
            default:
                sb.Append(ex.Message);
                if (ex.InnerException != null)
                {
                    sb.Append(" --> ").Append(ex.InnerException.AllMessages());
                }
                break;
        }
        return sb.ToString();
    }
    /// <summary>
    /// Extracts the headline message from an <see cref="AggregateException"/>, stripping the
    /// inner exception messages that <see cref="AggregateException.Message"/> appends automatically.
    /// </summary>
    private static string GetAggregateHeadline(AggregateException aggEx)
    {
        if (aggEx.InnerExceptions.Count == 0) return aggEx.Message;
        // AggregateException.Message format: "headline (inner1.Message) (inner2.Message)..."
        // Reconstruct the auto-appended suffix and strip it to recover the original headline.
        var suffix = new StringBuilder(" ");
        for (int i = 0; i < aggEx.InnerExceptions.Count; i++)
        {
            if (i > 0) suffix.Append(' ');
            suffix.Append('(').Append(aggEx.InnerExceptions[i].Message).Append(')');
        }
        var suffixStr = suffix.ToString();
        var message = aggEx.Message;
        return message.EndsWith(suffixStr, StringComparison.Ordinal)
            ? message[..^suffixStr.Length]
            : message;
    }
}