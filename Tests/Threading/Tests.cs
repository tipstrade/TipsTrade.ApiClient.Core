using TipsTrade.ApiClient.Core.Threading;

namespace Tests.Threading {
  public class Tests {
    [Test(Description = "GetCurrentCount throws for invalid key.")]
    public async Task GetCurrentCount_Throws_For_Invalid_Key() {
      var semaphore = new KeyedSemaphoreSlim<MyClassKey>(1);

      await semaphore.WaitAsync(new());

      Assert.Throws<InvalidOperationException>(() => semaphore.GetCurrentCount(new()));
    }

    [TestCaseSource(nameof(SemaphoreKeys))]
    public async Task Semaphore_Supports_Key<T>(T key) where T : notnull {
      var semaphore = new KeyedSemaphoreSlim<T>(1);

      try {
        await semaphore.WaitAsync(key);

        Assert.That(semaphore.GetCurrentCount(key), Is.Zero, $"CurrentCount invalid for {typeof(T)}");
      } finally {
        Assert.DoesNotThrow(() => semaphore.Release(key));
      }
    }

    [Test(Description = "WaitAsync throw for invalid count sizes.")]
    public void WaitAsync_Throws_For_Invalid_Counts() {
      var semaphore = new KeyedSemaphoreSlim<string>(1);

      Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => {
        await semaphore.WaitAsync("", 2, 1);
      });
    }

    [Test(Description = "WaitAsync throw for invalid key.")]
    public async Task WaitAsync_Throws_For_Invalid_Key() {
      var semaphore = new KeyedSemaphoreSlim<MyClassKey>(1);

      await semaphore.WaitAsync(new());

      Assert.Throws<InvalidOperationException>(() => semaphore.Release(new()));
    }

    #region Test cases
    internal static IEnumerable<TestCaseData> SemaphoreKeys() {
      yield return new TestCaseData(100)
        .SetName("Supports int key");
      yield return new TestCaseData("abc")
        .SetName("Supports string key");
      yield return new TestCaseData(("foo", "bar"))
        .SetName("Supports tuple key");
      yield return new TestCaseData(new MyClassKey { Tenant = "bob", Provider = "MOT" })
        .SetName("Supports class key");
      yield return new TestCaseData(new MyRecordKey(Tenant: "bob", Provider: "MOT"))
        .SetName("Supports record key");
      yield return new TestCaseData(new MyStructKey { Tenant = "bob", Provider = "MOT" })
        .SetName("Supports struct key");
    }
    #endregion

    #region Inner classes
    private class MyClassKey {
      public string Tenant { get; set; } = "";
      public string Provider { get; set; } = "";
    }

    private record MyRecordKey(string Tenant, string Provider);

    private struct MyStructKey {
      public string Tenant { get; set; }
      public string Provider { get; set; }
    }
    #endregion
  }
}
