namespace TipsTrade.ApiClient.Core.Credential {
  /// <summary>Defines methods for setting a strongly typed credential.</summary>
  /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
  /// <typeparam name="TCredential">The type of credential.</typeparam>
  public interface ISetCredential<TKey, TCredential> {
    /// <summary>Sets the specified credential.</summary>
    /// <param name="key">The key used to retrieve the credential.</param>
    /// <param name="credential">The credential instance.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns><c>true</c> if successful.</returns>
    Task SetCredentialAsync(TKey key, TCredential credential, CancellationToken cancellationToken = default);
  }
}
