using Moq;
using TipsTrade.ApiClient.Core.Caching;

namespace Tests.Caching {
  public class Tests {
    private static readonly DateTime ExpectedDate = new DateTime(2025, 7, 25);
    private static readonly int ExpectedInt = 1234;

    private Mock<IReadWriteCache<string, DateTime?>> typedCache;
    private Mock<IReadWriteCache<string>> untypedCache;

    [SetUp]
    public void SetUp() {
      typedCache = new Mock<IReadWriteCache<string, DateTime?>>();

      typedCache.Setup(x => x.AddToCacheAsync("success", It.IsAny<DateTime>(), default));
      typedCache.Setup(x => x.AddToCacheAsync("failure", It.IsAny<DateTime>(), default)).Throws<Exception>();
      typedCache.Setup(x => x.GetFromCacheAsync("hit", default)).ReturnsAsync(ExpectedDate);
      typedCache.Setup(x => x.GetFromCacheAsync("miss", default)).ReturnsAsync((DateTime?)null);
      typedCache.Setup(x => x.GetFromCacheAsync("failure", default)).Throws<Exception>();

      untypedCache = new Mock<IReadWriteCache<string>>();

      untypedCache.Setup(x => x.AddToCacheAsync("add-date", It.IsAny<DateTime>(), default));
      untypedCache.Setup(x => x.AddToCacheAsync("failure", It.IsAny<DateTime>(), default)).Throws<Exception>();
      untypedCache.Setup(x => x.GetFromCacheAsync<DateTime?>("hit-date", default)).ReturnsAsync(ExpectedDate);
      untypedCache.Setup(x => x.GetFromCacheAsync<int?>("hit-int", default)).ReturnsAsync(ExpectedInt);
      untypedCache.Setup(x => x.GetFromCacheAsync<DateTime?>("miss", default)).ReturnsAsync((DateTime?)null);
      untypedCache.Setup(x => x.GetFromCacheAsync<DateTime?>("failure", default)).Throws<Exception>();
    }

    [Test]
    public void TryAddToCacheAsync_Fails_With_False() {
      Assert.ThatAsync(() => typedCache.Object.TryAddToCacheAsync("failure", ExpectedDate), Is.False);
      typedCache.Verify(x => x.AddToCacheAsync("failure", ExpectedDate, It.IsAny<CancellationToken>()), Times.Once);

      Assert.ThatAsync(() => untypedCache.Object.TryAddToCacheAsync("failure", ExpectedDate), Is.False);
      untypedCache.Verify(x => x.AddToCacheAsync("failure", ExpectedDate, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void TryAddToCacheAsync_Succeeds_With_True() {
      Assert.ThatAsync(() => typedCache.Object.TryAddToCacheAsync("success", ExpectedDate), Is.True);
      typedCache.Verify(x => x.AddToCacheAsync("success", ExpectedDate, It.IsAny<CancellationToken>()), Times.Once);

      Assert.ThatAsync(() => untypedCache.Object.TryAddToCacheAsync("add-date", ExpectedDate), Is.True);
      untypedCache.Verify(x => x.AddToCacheAsync("add-date", ExpectedDate, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void TryGetFromCacheAsync_Fails_With_Null() {
      Assert.ThatAsync(() => typedCache.Object.TryGetFromCacheAsync("failure"), Is.Null);
      typedCache.Verify(x => x.GetFromCacheAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

      Assert.ThatAsync(() => untypedCache.Object.TryGetFromCacheAsync<string, DateTime?>("failure"), Is.Null);
      untypedCache.Verify(x => x.GetFromCacheAsync<DateTime?>("failure", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void TryGetFromCacheAsync_Hit_With_DateTime() {
      Assert.ThatAsync(() => typedCache.Object.TryGetFromCacheAsync("hit"), Is.EqualTo(ExpectedDate));
      typedCache.Verify(x => x.GetFromCacheAsync("hit", It.IsAny<CancellationToken>()), Times.Once);

      Assert.ThatAsync(() => untypedCache.Object.TryGetFromCacheAsync<string, DateTime?>("hit-date"), Is.EqualTo(ExpectedDate));
      untypedCache.Verify(x => x.GetFromCacheAsync<DateTime?>("hit-date", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void TryGetFromCacheAsync_Miss_With_Null() {
      Assert.ThatAsync(() => typedCache.Object.TryGetFromCacheAsync("miss"), Is.Null);
      typedCache.Verify(x => x.GetFromCacheAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

      Assert.ThatAsync(() => untypedCache.Object.TryGetFromCacheAsync<string, DateTime?>("miss"), Is.Null);
      untypedCache.Verify(x => x.GetFromCacheAsync<DateTime?>("miss", It.IsAny<CancellationToken>()), Times.Once);
    }
  }
}
