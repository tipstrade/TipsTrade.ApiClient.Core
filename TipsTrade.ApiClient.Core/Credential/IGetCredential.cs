namespace TipsTrade.ApiClient.Core.Credential {
  /// <summary>Defines methods for retrieving a credential.</summary>
  public interface IGetCredential { }

  /// <summary>Defines methods for retrieving a strongly typed credential.</summary>
  /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
  /// <typeparam name="T">The type of credential.</typeparam>
  public interface IGetCredential<TKey, T> : IGetCredential where T : class, new() {
    /// <summary>Gets the specified credential.</summary>
    /// <param name="key">The key used to retrieve the credential.</param>
    /// <param name="ct">A token to monitor for cancellation requests.</param>
    Task<CredentialResponse<T>> GetCredentialAsync(TKey key, CancellationToken ct);
  }

  /// <summary>Defines methods for retrieving a credential.</summary>
  /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
  public interface IGetGenericCredential<TKey> : IGetCredential {
    /// <summary>Gets the specified credential.</summary>
    /// <typeparam name="T">The type of credential.</typeparam>
    /// <param name="key">The key used to retrieve the credential.</param>
    /// <param name="ct">A token to monitor for cancellation requests.</param>
    Task<CredentialResponse<T>> GetCredentialAsync<T>(TKey key, CancellationToken ct) where T : class, new();
  }
}
