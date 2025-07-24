namespace TipsTrade.ApiClient.Core.Error {
  /// <summary>Provides helper methods for generating consistent exceptions.</summary>
  public static class ExceptionFactory {
    /// <summary>Gets the exception when an error occurred adding an item to the cache.</summary>
    /// <typeparam name="TKey">The type of key used to identify the item.</typeparam>
    /// <typeparam name="TValue">The type of item in the cache.</typeparam>
    /// <param name="key">The key used to identify the item.</param>
    /// <param name="item">The item to add to the cache.</param>
    /// <param name="exception">The original exception thrown.</param>
    public static Exception AddToCacheFailed<TKey, TValue>(TKey key, TValue item, Exception exception) {
      var ex = new InvalidOperationException("An error occurred adding the cache item.", exception);

      ex.Data.Add(nameof(key), key);
      ex.Data.Add(nameof(item), item);

      return ex;
    }

    /// <summary>Gets the exception when an error occurred retrieving a credential.</summary>
    /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
    /// <param name="key">The key used to identify the credential.</param>
    /// <param name="exception">The original exception thrown.</param>
    public static Exception GetCredentialFailed<TKey>(TKey key, Exception exception) {
      var ex = new InvalidOperationException("An error occurred retrieving the credential.", exception);

      ex.Data.Add(nameof(key), key);

      return ex;
    }

    /// <summary>Gets the exception when an error occurred retrieving an item from the cache.</summary>
    /// <typeparam name="TKey">The type of key used to identify the item.</typeparam>
    /// <param name="key">The key used to identify the item.</param>
    /// <param name="exception">The original exception thrown.</param>
    public static Exception GetFromCacheFailed<TKey>(TKey key, Exception exception) {
      var ex = new InvalidOperationException("An error occurred retrieving the cache item.", exception);

      ex.Data.Add(nameof(key), key);

      return ex;
    }

    /// <summary>Gets the exception when an error occurred retrieving a tenant.</summary>
    /// <param name="exception">The original exception thrown.</param>
    public static Exception GetTenantFailed(Exception exception) {
      return new InvalidOperationException("An error occurred retrieving the tenant.", exception);
    }

    /// <summary>Gets the exception when an error occurred setting a credential.</summary>
    /// <typeparam name="TKey">The type of key used to identify the credential.</typeparam>
    /// <typeparam name="TCredential">The type of credential.</typeparam>
    /// <param name="key">The key used to identify the credential.</param>
    /// <param name="credential">The credential instance.</param>
    /// <param name="exception">The original exception thrown.</param>
    public static Exception SetCredentialFailed<TKey, TCredential>(TKey key, TCredential credential, Exception exception) {
      var ex = new InvalidOperationException("An error occurred setting the credential.", exception);

      ex.Data.Add(nameof(key), key);
      ex.Data.Add(nameof(credential), credential);

      return ex;
    }
  }
}
