namespace TipsTrade.ApiClient.Core.Credential {
  /// <summary>Defines methods for setting a strongly typed credential.</summary>
  /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
  /// <typeparam name="T">The type of credential.</typeparam>
  public interface ISetCredential<TKey, T> where T : class, new() {
    /// <summary>Sets the specified credential.</summary>
    /// <param name="key">The key used to retrieve the credential.</param>
    /// <param name="credential">The credential instance.</param>
    /// <param name="ct">A token to monitor for cancellation requests.</param>
    /// <returns><c>true</c> if successful.</returns>
    Task<bool> SetCredentialAsync(TKey key, T credential, CancellationToken ct);
  }
}
