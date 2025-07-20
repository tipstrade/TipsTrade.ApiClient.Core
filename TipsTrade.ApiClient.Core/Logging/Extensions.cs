using Microsoft.Extensions.Logging;

namespace TipsTrade.ApiClient.Core.Logging {
  public static class Extensions {
    /// <summary>Gets the optional <see cref="ILogger"/> for the current values.</summary>
    public static ILogger? GetLogger<T>(this T? value) => value is IWithLoger logger ? logger.Logger : null;

    /// <summary>Writes a log entry, if the specified logLevel is met.</summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="message">The function that generates the message if logging is enabled.</param>
    public static void LogIf(this ILogger? logger, LogLevel logLevel, Func<string> message) {
      if (logger?.IsEnabled(logLevel) == true) {
        logger.Log(logLevel, message.Invoke());
      }
    }
  }
}