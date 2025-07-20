using Microsoft.Extensions.Logging;

namespace TipsTrade.ApiClient.Core.Logging {
  /// <summary>Defines logger properties.</summary>
  public interface IWithLoger {
    /// <summary>The <see cref="ILogger"/> reference.</summary>
    ILogger Logger { get; }
  }
}
