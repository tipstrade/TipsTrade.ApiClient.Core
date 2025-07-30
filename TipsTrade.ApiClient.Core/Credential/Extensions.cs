using Microsoft.Extensions.Logging;
using TipsTrade.ApiClient.Core.Error;
using TipsTrade.ApiClient.Core.Logging;

namespace TipsTrade.ApiClient.Core.Credential {
  /// <summary>Provides extension methods for the <see cref="Credential"/> namespace.</summary>
  public static class Extensions {
    /// <summary>
    /// Retrieves the specified credential or throws an exception if retrieval fails.
    /// </summary>
    /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
    /// <typeparam name="TCredential">The type of credential.</typeparam>
    /// <param name="provider">The credential provider.</param>
    /// <param name="key">The key used to retrieve the credential.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The retrieved credential.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the credential cannot be retrieved.</exception>
    public static async Task<TCredential> GetCredentialOrThrowAsync<TKey, TCredential>(this IGetCredential<TKey, TCredential> provider, TKey key, CancellationToken cancellationToken = default) {
      try {
        return await provider.GetCredentialAsync(key, cancellationToken);
      } catch (Exception ex) {
        provider.GetLogger()?.LogError("Failed to retrieve credential for Key={key}: {message}", key, ex.Message);
        throw ExceptionFactory.GetCredentialFailed(key, ex);
      }
    }

    /// <summary>
    /// Retrieves the specified credential or throws an exception if retrieval fails.
    /// </summary>
    /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
    /// <typeparam name="TCredential">The type of credential.</typeparam>
    /// <param name="provider">The credential provider.</param>
    /// <param name="key">The key used to retrieve the credential.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The retrieved credential.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the credential cannot be retrieved.</exception>
    public static async Task<TCredential> GetCredentialOrThrowAsync<TKey, TCredential>(this IGetCredential<TKey> provider, TKey key, CancellationToken cancellationToken = default) {
      try {
        return await provider.GetCredentialAsync<TCredential>(key, cancellationToken);
      } catch (Exception ex) {
        provider.GetLogger()?.LogError("Failed to retrieve credential for Key={key}: {message}", key, ex.Message);
        throw ExceptionFactory.GetCredentialFailed(key, ex);
      }
    }

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
        value.GetLogger()?.LogError("Failed to set credential for Key={key}: {message}", key, ex.Message);

        return false;
      }
    }
  }
}
