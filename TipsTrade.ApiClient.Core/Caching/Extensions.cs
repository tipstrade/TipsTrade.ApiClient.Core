using Microsoft.Extensions.Logging;
using TipsTrade.ApiClient.Core.Logging;

namespace TipsTrade.ApiClient.Core.Caching {
  /// <summary>Provides extension methods for the <see cref="Caching"/> namespace.</summary>
  public static class Extensions {
    /// <summary>Attempts to add the item to the cache.</summary>
    /// <typeparam name="TKey">The type of key used to identify the item.</typeparam>
    /// <typeparam name="TValue">The type of item in the cache.</typeparam>
    /// <param name="cache">The current cache.</param>
    /// <param name="key">The key used to identify the item.</param>
    /// <param name="item">The item to add to the cache.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns><c>true</c> if successful.</returns>
    public static async Task<bool> TryAddToCacheAsync<TKey, TValue>(this IAddToCache<TKey> cache, TKey key, TValue item, CancellationToken cancellationToken = default) {
      try {
        await cache.AddToCacheAsync(key, item, cancellationToken);

        return true;
      } catch (Exception ex) {
        cache.GetLogger()?.LogError("Failed to add to the cache for Key={key}: {message}", key, ex.Message);
      }

      return false;
    }

    /// <summary>Attempts to add the item to the cache.</summary>
    /// <typeparam name="TKey">The type of key used to identify the item.</typeparam>
    /// <typeparam name="TValue">The type of item in the cache.</typeparam>
    /// <param name="cache">The current cache.</param>
    /// <param name="key">The key used to identify the item.</param>
    /// <param name="item">The item to add to the cache.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns><c>true</c> if successful.</returns>
    public static async Task<bool> TryAddToCacheAsync<TKey, TValue>(this IAddToCache<TKey, TValue> cache, TKey key, TValue item, CancellationToken cancellationToken = default) {
      try {
        await cache.AddToCacheAsync(key, item, cancellationToken);

        return true;
      } catch (Exception ex) {
        cache.GetLogger()?.LogError("Failed to add to the cache for Key={key}: {message}", key, ex.Message);
      }

      return false;
    }

    /// <summary>Attempts to get the item from the cache.</summary>
    /// <typeparam name="TKey">The type of key used to identify the item.</typeparam>
    /// <typeparam name="TValue">The type of item in the cache.</typeparam>
    /// <param name="cache">The current cache.</param>
    /// <param name="key">The key used to identify the item.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The cached item, or <c>null</c> if not found.</returns>
    public static async Task<TValue?> TryGetFromCacheAsync<TKey, TValue>(this IGetFromCache<TKey> cache, TKey key, CancellationToken cancellationToken = default) {
      try {
        var value = await cache.GetFromCacheAsync<TValue>(key, cancellationToken);

        if (value is TValue) {
          cache.GetLogger()?.LogTrace("Hit from cache for Key={key}", key);

          return value;
        }

        cache.GetLogger()?.LogTrace("Miss from cache for Key={key}", key);
      } catch (Exception ex) {
        cache.GetLogger()?.LogError("Failed to retrieve from the cache for Key={key}: {message}", key, ex.Message);
      }

      return default;
    }

    /// <summary>Attempts to get the item from the cache.</summary>
    /// <typeparam name="TKey">The type of key used to identify the item.</typeparam>
    /// <typeparam name="TValue">The type of item in the cache.</typeparam>
    /// <param name="cache">The current cache.</param>
    /// <param name="key">The key used to identify the item.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The cached item, or <c>null</c> if not found.</returns>
    public static async Task<TValue?> TryGetFromCacheAsync<TKey, TValue>(this IGetFromCache<TKey, TValue> cache, TKey key, CancellationToken cancellationToken = default) {
      try {
        var value = await cache.GetFromCacheAsync(key, cancellationToken);

        if (value is TValue) {
          cache.GetLogger()?.LogTrace("Hit from cache for Key={key}", key);

          return value;
        }

        cache.GetLogger()?.LogTrace("Miss from cache for Key={key}", key);
      } catch (Exception ex) {
        cache.GetLogger()?.LogError("Failed to retrieve from the cache for Key={key}: {message}", key, ex.Message);
      }

      return default;
    }
  }
}
