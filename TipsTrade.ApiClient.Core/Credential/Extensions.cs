using Microsoft.Extensions.Logging;
using TipsTrade.ApiClient.Core.Logging;

namespace TipsTrade.ApiClient.Core.Credential {
  /// <summary>Provides extension methods for the <see cref="Credential"/> namespace.</summary>
  public static class Extensions {
    /// <summary>Attempts to set the specified credential.</summary>
    /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
    /// <typeparam name="TCredential">The type of credential.</typeparam>
    /// <param name="value">The current value.</param>
    /// <param name="key">The key used to retrieve the credential.</param>
    /// <param name="credential">The credential.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns><c>true</c> is successful.</returns>
    public static async Task<bool> TrySetCredentialAsync<TKey, TCredential>(this ISetCredential<TKey, TCredential> value, TKey key, TCredential credential, CancellationToken cancellationToken = default) {
      try {
        await value.SetCredentialAsync(key, credential, cancellationToken);

        return true;
      } catch (Exception ex) {
        value.GetLogger()?.LogError("Failed to set credential: {message}", ex.Message);

        return false;
      }
    }
  }
}
