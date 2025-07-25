namespace TipsTrade.ApiClient.Core.Caching {
  /// <summary>Defines methods for retrieving items from a cache.</summary>
  /// <typeparam name="TKey">The type of key used to identify the item.</typeparam>
  public interface IAddToCache<TKey> {
    /// <summary>Gets the item from the cache.</summary>
    /// <typeparam name="TValue">The type of item in the cache.</typeparam>
    /// <param name="key">The key used to identify the item.</param>
    /// <param name="item">The item to add to the cache.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task AddToCacheAsync<TValue>(TKey key, TValue item, CancellationToken cancellationToken = default);
  }

  /// <summary>Defines methods for retrieving items from a cache.</summary>
  /// <typeparam name="TKey">The type of key used to identify the item.</typeparam>
  /// <typeparam name="TValue">The type of item in the cache.</typeparam>
  public interface IAddToCache<TKey, TValue> {
    /// <summary>Gets the item from the cache.</summary>
    /// <param name="key">The key used to identify the item.</param>
    /// <param name="item">The item to add to the cache.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task AddToCacheAsync(TKey key, TValue item, CancellationToken cancellationToken = default);
  }
}
