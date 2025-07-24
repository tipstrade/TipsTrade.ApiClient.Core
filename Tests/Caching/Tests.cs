using Moq;
using TipsTrade.ApiClient.Core.Caching;

namespace Tests.Caching {
  public class Tests {
    private Mock<IReadWriteCache<string, DateTime?>> cache;

    [SetUp]
    public void SetUp() {
      cache = new Mock<IReadWriteCache<string, DateTime?>>();

      cache.Setup(x => x.AddToCacheAsync("success", It.IsAny<DateTime>(), default));
      cache.Setup(x => x.AddToCacheAsync("failure", It.IsAny<DateTime>(), default)).Throws<Exception>();
      cache.Setup(x => x.GetFromCacheAsync("hit", default)).ReturnsAsync(new DateTime());
      cache.Setup(x => x.GetFromCacheAsync("miss", default)).ReturnsAsync((DateTime?)null);
      cache.Setup(x => x.GetFromCacheAsync("failure", default)).Throws<Exception>();
    }

    [Test]
    public void TryAddToCacheAsync_Succeeds_On_Error() {
      Assert.ThatAsync(() => cache.Object.TryAddToCacheAsync("success", DateTime.Now), Is.True);
      cache.Verify(x => x.AddToCacheAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void TryAddToCacheAsync_Succeeds_On_Success() {
      Assert.ThatAsync(() => cache.Object.TryAddToCacheAsync("failure", DateTime.Now), Is.False);
      cache.Verify(x => x.AddToCacheAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void TryGetFromCacheAsync_Succeeds_On_Failure() {
      Assert.ThatAsync(() => cache.Object.TryGetFromCacheAsync("failure"), Is.Null);
      cache.Verify(x => x.GetFromCacheAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void TryGetFromCacheAsync_Succeeds_On_Hit() {
      Assert.ThatAsync(() => cache.Object.TryGetFromCacheAsync("hit"), Is.InstanceOf<DateTime>());
      cache.Verify(x => x.GetFromCacheAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void TryGetFromCacheAsync_Succeeds_On_Miss() {
      Assert.ThatAsync(() => cache.Object.TryGetFromCacheAsync("miss"), Is.Null);
      cache.Verify(x => x.GetFromCacheAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
  }
}
