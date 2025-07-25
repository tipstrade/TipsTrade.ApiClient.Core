namespace TipsTrade.ApiClient.Core.Caching {
  /// <summary>Defines methods for a read-write cache.</summary>
  /// <typeparam name="TKey">The type of key used to identify the item.</typeparam>
  public interface IReadWriteCache<TKey> : IAddToCache<TKey>, IGetFromCache<TKey> {
  }

  /// <summary>Defines methods for a read-write cache.</summary>
  /// <typeparam name="TKey">The type of key used to identify the item.</typeparam>
  /// <typeparam name="TValue">The type of item in the cache.</typeparam>
  public interface IReadWriteCache<TKey, TValue> : IAddToCache<TKey, TValue>, IGetFromCache<TKey, TValue> {
  }
}
