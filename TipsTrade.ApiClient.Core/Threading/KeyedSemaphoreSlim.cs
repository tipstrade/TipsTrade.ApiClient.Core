using System.Collections.Concurrent;

namespace TipsTrade.ApiClient.Core.Threading {
  /// <summary>Represents a thread-safe keyed <see cref="SemaphoreSlim"/>.</summary>
  /// <typeparam name="K">The type of key.</typeparam>
  /// <remarks>
  /// Semaphores are not automatically disposed. This class is designed for long-lived, singleton-style use.
  /// If manual cleanup is needed, remove specific keys using your own logic or consider extending the class.
  /// </remarks>
  public class KeyedSemaphoreSlim<K> where K : notnull {
    #region Fields
    private readonly ConcurrentDictionary<K, SemaphoreSlim> semaphores = new();
    #endregion

    #region Properties
    /// <summary>The default initial number of requests for the semaphore that can be granted concurrently.</summary>
    /// <remarks>This will not affect any existing semaphores.</remarks>
    public int DefaultInitialCount { get; }

    /// <summary>The default maximum number of requests for the semaphore that can be granted concurrently.</summary>
    /// <remarks>This will not affect any existing semaphores.</remarks>
    public int DefaultMaxCount { get; }
    #endregion

    #region Lifecycle
    /// <summary>Creates an instance of the <see cref="KeyedSemaphoreSlim{K}"/> class.</summary>
    /// <param name="defaultInitialCount">The default initial number of requests for the semaphore that can be granted concurrently.</param>
    /// <param name="defaultMaxCount">The default maximum number of requests for the semaphore that can be granted concurrently.</param>
    public KeyedSemaphoreSlim(int defaultInitialCount, int defaultMaxCount = int.MaxValue) {
      DefaultInitialCount = defaultInitialCount;
      DefaultMaxCount = defaultMaxCount;
    }
    #endregion

    #region Methods
    /// <summary>Determines whether the specified key exists in the dictionary.</summary>
    public bool ContainsKey(K key) => semaphores.ContainsKey(key);

    /// <summary>Gets the current count.</summary>
    /// <param name="key">The key for the semaphore.</param>
    /// <returns>The current count.</returns>
    /// <exception cref="InvalidOperationException">If the key doesn't exist.</exception>
    public int GetCurrentCount(K key) {
      if (semaphores.TryGetValue(key, out var semaphore)) {
        return semaphore.CurrentCount;
      } else {
        throw new InvalidOperationException($"Couldn't find a semaphore for {key}");
      }
    }

    /// <summary>Exits the <see cref="SemaphoreSlim"/> once.</summary>
    /// <remarks>
    /// The store is backed by a dictionary, so when using reference types be sure to pass the same object reference.
    /// </remarks>
    /// <param name="key">The key for the semaphore.</param>
    /// <exception cref="InvalidOperationException">If the key doesn't exist.</exception>
    public void Release(K key) {
      if (!TryRelease(key)) {
        throw new InvalidOperationException($"Couldn't find a semaphore for {key}");
      }
    }

    /// <summary>Attempts to release the semaphore with the specified key.</summary>
    /// <param name="key">The key for the semaphore.</param>
    /// <returns><c>true</c> if the semaphore was found and successfully released; otherwise, <c>false</c>.</returns>
    public bool TryRelease(K key) {
      if (semaphores.TryGetValue(key, out var semaphore)) {
        semaphore.Release();

        return true;
      }

      return false;
    }

    /// <summary>Asynchronously waits to enter the <see cref="SemaphoreSlim"/>.</summary>
    /// <param name="key">The key for the semaphore.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that will complete when the semaphore has been entered.</returns>
    public Task WaitAsync(K key, CancellationToken cancellationToken = default) {
      return WaitAsync(key, DefaultInitialCount, DefaultMaxCount, cancellationToken);
    }

    /// <summary>Asynchronously waits to enter the <see cref="SemaphoreSlim"/>.</summary>
    /// <param name="key">The key for the semaphore.</param>
    /// <param name="initialCount">The initial number of requests for the semaphore that can be granted concurrently.</param>
    /// <param name="maxCount">The maximum number of requests for the semaphore that can be granted concurrently.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that will complete when the semaphore has been entered.</returns>
    public async Task WaitAsync(K key, int initialCount, int maxCount = int.MaxValue, CancellationToken cancellationToken = default) {
      var semaphore = semaphores.GetOrAdd(key, _ => new SemaphoreSlim(initialCount, maxCount));

      await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
    }
    #endregion
  }
}
