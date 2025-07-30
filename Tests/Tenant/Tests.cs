using Moq;
using System.Xml.Serialization;
using TipsTrade.ApiClient.Core.Tenant;

namespace Tests.Tenant {
  public class Tests {
    private static Mock<IGetTenant<T>> GetFailureMock<T, TException>() where TException : Exception, new() {
      var mock = new Mock<IGetTenant<T>>();

      mock.Setup(x => x.GetTenantAsync(It.IsAny<CancellationToken>()))
        .Throws<TException>();

      return mock;
    }

    private static Mock<IGetTenant<T>> GetSuccessMock<T>(T tenantValue) {
      var mock = new Mock<IGetTenant<T>>();

      mock.Setup(x => x.GetTenantAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(tenantValue);

      return mock;
    }

    [Test(Description = "GetTenantOrDefault<int> succeeds with a null reference")]
    public async Task GetIntTenantOrDefault_Succeeds_For_Null() {
      IGetTenant<int>? mock = null;
      var expected = -1;

      var actual = await mock.GetTenantOrDefaultAsync(expected);

      Assert.That(actual, Is.EqualTo(expected));
    }

    [Test(Description = "GetTenantOrDefault<string> succeeds with a null reference")]
    public async Task GetStringTenantOrDefault_Succeeds_For_Null() {
      var expected = "(default)";
      string actual;
      IGetTenant<string>? mock = null;

      // With a default value
      expected = "None";
      actual = await mock.GetTenantOrDefaultAsync("None");

      Assert.That(actual, Is.EqualTo(expected));
    }

    [Test(Description = "GetTenantOrDefault succeeds with a null reference")]
    public async Task GetTenantOrDefault_Succeeds_For_Null() {
      var expected = "(default)";
      string actual;
      IGetTenant? mock = null;

      // With no default
      actual = await mock.GetTenantOrDefaultAsync();

      Assert.That(actual, Is.EqualTo(expected));
    }

    [Test(Description = "GetTenantOrDefaultAsync succeeds")]
    public async Task GetTenantOrDefaultAsync_Succeeds() {
      var expected = "Bob";
      var mock = GetSuccessMock(expected);

      var actual = await mock.Object.GetTenantOrDefaultAsync("(default)");

      Assert.That(actual, Is.EqualTo(expected));
      mock.Verify(x => x.GetTenantAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test(Description = "GetTenantOrDefaultAsync throws")]
    public void GetTenantOrDefaultAsync_Throws() {
      var mock = GetFailureMock<string, InvalidOperationException>();

      Assert.ThrowsAsync<InvalidOperationException>(() => mock.Object.GetTenantOrDefaultAsync("(default)"));
      mock.Verify(x => x.GetTenantAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test(Description = "GetTenantOrThrowAsync succeeds")]
    public async Task GetTenantOrThrowAsync_Succeeds() {
      var mock = GetSuccessMock("(default)");

      var actual = await mock.Object.GetTenantOrThrowAsync("(default)");
      mock.Verify(x => x.GetTenantAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test(Description = "GetTenantOrThrowAsync throws")]
    public void GetTenantOrThrowAsync_Throws() {
      var mock = GetFailureMock<string, ArgumentException>();

      Assert.ThrowsAsync<InvalidOperationException>(() => mock.Object.GetTenantOrThrowAsync("(default)"));
      mock.Verify(x => x.GetTenantAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
  }
}