using Microsoft.Extensions.Logging;
using TipsTrade.ApiClient.Core.Logging;

namespace TipsTrade.ApiClient.Core.Credential {
  public static class Extensions {
    /// <summary>Attempts to get the specified credential.</summary>
    /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
    /// <typeparam name="T">The type of credential to be retrieved.</typeparam>
    /// <param name="value">The current value.</param>
    /// <param name="key">The key used to retrieve the credential.</param>
    /// <param name="ct">A token to monitor for cancellation requests.</param>
    /// <returns>The credential, or <c>null</c> if invalid.</returns>
    public static Task<CredentialResponse<T>> TryGetCredentialAsync<TKey, T>(this IGetCredential<TKey, T> value, TKey key, CancellationToken ct = default) where T : class, new() {
      return TryGetCredentialInternalAsync<TKey, T>(value, key, ct);
    }

    /// <summary>Attempts to get the specified credential.</summary>
    /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
    /// <typeparam name="T">The type of credential to be retrieved.</typeparam>
    /// <param name="value">The current value.</param>
    /// <param name="key">The key used to retrieve the credential.</param>
    /// <param name="ct">A token to monitor for cancellation requests.</param>
    /// <returns>The credential, or <c>null</c> if invalid.</returns>
    public static Task<CredentialResponse<T>> TryGetCredentialAsync<TKey, T>(this IGetGenericCredential<TKey> value, TKey key, CancellationToken ct = default) where T : class, new() {
      return TryGetCredentialInternalAsync<TKey, T>(value, key, ct);
    }

    internal static async Task<CredentialResponse<T>> TryGetCredentialInternalAsync<TKey, T>(this IGetCredential value, TKey key, CancellationToken ct = default) where T : class, new() {
      try {
        if (value is IGetGenericCredential<TKey> generic) {
          return await generic.GetCredentialAsync<T>(key, ct);
        } else if (value is IGetCredential<TKey, T> credentials) {
          return await credentials.GetCredentialAsync(key, ct);
        } else {
          // Something has seriously gone wrong here
          value.GetLogger()?.LogError("{type} cannot provide credentials.", value.GetType());

          return $"{value.GetType()} cannot provide credentials";
        }
      } catch (Exception ex) {
        value.GetLogger()?.LogError("Failed to get credentials: {message}", ex.Message);

        return ex;
      }
    }

    /// <summary>Attempts to set the specified credential.</summary>
    /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
    /// <typeparam name="T">The type of credential.</typeparam>
    /// <param name="value">The current value.</param>
    /// <param name="key">The key used to retrieve the credential.</param>
    /// <param name="credential">The credential.</param>
    /// <param name="ct">A token to monitor for cancellation requests.</param>
    /// <returns><c>true</c> is successful.</returns>
    public static async Task<bool> TrySetCredentialAsync<TKey, T>(this ISetCredential<TKey, T> value, TKey key, T credential, CancellationToken ct = default) where T : class, new() {
      try {
        return await value.SetCredentialAsync(key, credential, ct);
      } catch (Exception ex) {
        value.GetLogger()?.LogError("Failed to set credential: {message}", ex.Message);

        return false;
      }
    }
  }
}
