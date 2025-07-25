using Microsoft.Extensions.Logging;

namespace TipsTrade.ApiClient.Core.Logging {
  /// <summary>Defines logger properties.</summary>
  public interface IWithLogger {
    /// <summary>The <see cref="ILogger"/> reference.</summary>
    ILogger Logger { get; }
  }
}
