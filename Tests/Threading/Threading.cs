using System.Threading.Tasks;
using TipsTrade.ApiClient.Core.Threading;

namespace Tests.Threading {
  public class Threading {
    private static KeyedSemaphoreSlim<T> CreateSemaphore<T>(int count = 1) where T : notnull => new(count);

    private static async Task TestSemaphore<T>(Func<T> key) where T : notnull {
      var semaphore = CreateSemaphore<T>();
      var k = key();

      try {
        await semaphore.WaitAsync(k);

        Assert.That(semaphore.GetCurrentCount(k), Is.EqualTo(0), $"CurrentCount invalid for {typeof(T)}");
      } finally {
        Assert.DoesNotThrow(() => semaphore.Release(k));
      }
    }

    [Test]
    public async Task SupportsMultipleKeys() {
      await TestSemaphore(() => 100);
      await TestSemaphore(() => "abc");
      await TestSemaphore(() => ("foo", "bar"));
      await TestSemaphore(() => new MyClassKey { Tenant = "foo", Provider = "bar" });
      await TestSemaphore(() => new MyRecordKey("foo", "bar"));
      await TestSemaphore(() => new MyStructKey { Tenant = "foo", Provider = "bar" });
    }

    [Test]
    public void WaitAsyncThrows() {
      var semaphore = CreateSemaphore<string>();

      Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => {
        await semaphore.WaitAsync("", 2, 1);
      });
    }

    private class MyClassKey {
      public string Tenant { get; set; }
      public string Provider { get; set; }
    }

    private record MyRecordKey(string Tenant, string Provider);

    private struct MyStructKey {
      public string Tenant { get; set; }
      public string Provider { get; set; }
    }
  }
}
