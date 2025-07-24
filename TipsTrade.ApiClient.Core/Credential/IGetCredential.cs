namespace TipsTrade.ApiClient.Core.Credential {
  /// <summary>Defines methods for retrieving a credential.</summary>
  /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
  public interface IGetCredential<TKey> {
    /// <summary>Gets the specified credential.</summary>
    /// <typeparam name="TCredential">The type of credential.</typeparam>
    /// <param name="key">The key used to identify the credential.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task<TCredential> GetCredentialAsync<TCredential>(TKey key, CancellationToken cancellationToken = default);
  }

  /// <summary>Defines methods for retrieving a strongly typed credential.</summary>
  /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
  /// <typeparam name="TCredential">The type of credential.</typeparam>
  public interface IGetCredential<TKey, TCredential> {
    /// <summary>Gets the specified credential.</summary>
    /// <param name="key">The key used to identify the credential.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task<TCredential> GetCredentialAsync(TKey key, CancellationToken cancellationToken = default);
  }
}
