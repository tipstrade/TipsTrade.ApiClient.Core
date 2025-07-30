using Moq;
using System.Threading.Tasks;
using TipsTrade.ApiClient.Core.Credential;

namespace Tests.Credential {
  public class Tests {
    [TestCaseSource(nameof(IsValidFalseCases))]
    public void IIsValid_False(IIsValid value) {
      Assert.That(value.IsValid, Is.False);
    }

    [TestCaseSource(nameof(IsValidTrueCases))]
    public void IIsValid_True(IIsValid value) {
      Assert.That(value.IsValid, Is.True);
    }

    [Test(Description = "TrySetCredentialAsync succeeds.")]
    public void TrySetCredentialAsync() {
      var mock = new Mock<ISetCredential<string, UserCredential>>();

      var success = new UserCredential("bob", "password");
      var fail = new UserCredential("", "");

      mock.Setup(x => x.SetCredentialAsync(It.IsAny<string>(), success, It.IsAny<CancellationToken>()));
      mock.Setup(x => x.SetCredentialAsync(It.IsAny<string>(), fail, It.IsAny<CancellationToken>())).Throws<InvalidOperationException>();

      Assert.ThatAsync(async () => await mock.Object.TrySetCredentialAsync("", success), Is.True);
      mock.Verify(x => x.SetCredentialAsync(It.IsAny<string>(), success, It.IsAny<CancellationToken>()), Times.Once);

      mock.Invocations.Clear();
      Assert.ThatAsync(async () => await mock.Object.TrySetCredentialAsync("", fail), Is.False);
      mock.Verify(x => x.SetCredentialAsync(It.IsAny<string>(), fail, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test(Description = "GetTenantOrThrowAsync succeeds")]
    public async Task GetTenantOrThrowAsync_Succeeds() {
      var expected = new ApiKeyCredential();
      var mock = new Mock<IGetCredential<string, ApiKeyCredential>>();

      mock.Setup(x => x.GetCredentialAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);

      var actual = await mock.Object.GetCredentialOrThrowAsync("");

      Assert.That(actual, Is.EqualTo(expected));
      mock.Verify(x => x.GetCredentialAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test(Description = "GetTenantOrThrowAsync<T> succeeds")]
    public async Task GetTenantOrThrowAsync_T_Succeeds() {
      var expected = new ApiKeyCredential();
      var mock = new Mock<IGetCredential<string>>();

      mock.Setup(x => x.GetCredentialAsync<ApiKeyCredential>(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);

      var actual = await mock.Object.GetCredentialOrThrowAsync<string, ApiKeyCredential>("");

      Assert.That(actual, Is.EqualTo(expected));
      mock.Verify(x => x.GetCredentialAsync<ApiKeyCredential>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test(Description = "GetTenantOrThrowAsync throws")]
    public void GetTenantOrThrowAsync_Throws() {
      var mock = new Mock<IGetCredential<string, ApiKeyCredential>>();

      mock.Setup(x => x.GetCredentialAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Throws<ArgumentException>();

      Assert.ThrowsAsync<InvalidOperationException>(() => mock.Object.GetCredentialOrThrowAsync("(default)"));
      mock.Verify(x => x.GetCredentialAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test(Description = "GetTenantOrThrowAsync<T> throws")]
    public void GetTenantOrThrowAsync_T_Throws() {
      var mock = new Mock<IGetCredential<string>>();

      mock.Setup(x => x.GetCredentialAsync<ApiKeyCredential>(It.IsAny<string>(), It.IsAny<CancellationToken>())).Throws<ArgumentException>();

      Assert.ThrowsAsync<InvalidOperationException>(() => mock.Object.GetCredentialOrThrowAsync<string, ApiKeyCredential>("(default)"));
      mock.Verify(x => x.GetCredentialAsync<ApiKeyCredential>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    #region Test cases
    internal static IEnumerable<TestCaseData> IsValidFalseCases() {
      yield return new TestCaseData(new ApiKeyCredential())
        .SetName("Is invalid with default API key");
      yield return new TestCaseData(new ApiKeyCredential("", "secret"))
        .SetName("Is invalid with empty API key");
      yield return new TestCaseData(new ApiKeyCredential(null!, "secret"))
        .SetName("Is invalid with null API key");
      yield return new TestCaseData(new UserCredential())
        .SetName("Is invalid with default User");
      yield return new TestCaseData(new UserCredential("", "password"))
        .SetName("Is invalid with empty username");
      yield return new TestCaseData(new UserCredential(null!, "password"))
        .SetName("Is invalid with null username");
      yield return new TestCaseData(new UserCredential("bob", ""))
        .SetName("Is invalid with empty password");
      yield return new TestCaseData(new UserCredential("bob", null!))
        .SetName("Is invalid with null password");
      // Overriden IsValid
      yield return new TestCaseData(new DummyCredential { Username = "", Password = "password", CompanyId = "a-1234" })
        .SetName("Is invalid with empty username with company");
      yield return new TestCaseData(new DummyCredential { Username = "alice", Password = "", CompanyId = "a-1234" })
        .SetName("Is invalid with empty password with company");
      yield return new TestCaseData(new DummyCredential { Username = "alice", Password = "password", CompanyId = "" })
        .SetName("Is invalid with empty company");
    }

    internal static IEnumerable<TestCaseData> IsValidTrueCases() {
      yield return new TestCaseData(new ApiKeyCredential("abcdef", ""))
        .SetName("Is valid with empty API secret");
      yield return new TestCaseData(new ApiKeyCredential("abcdef", null!))
        .SetName("Is valid with null API secret");
      yield return new TestCaseData(new ApiKeyCredential("abcdef", "secret"))
        .SetName("Is valid with non-empty secret");
      yield return new TestCaseData(new UserCredential("bob", "password"))
        .SetName("Is valid with username and password");
      // Overriden IsValid
      yield return new TestCaseData(new DummyCredential { Username = "alice", Password = "password", CompanyId = "a-1234" })
        .SetName("Is valid with with username, password and company");
    }
    #endregion

    #region Inner classes
    internal class DummyCredential : UserCredential {
      public string CompanyId { get; set; } = "";

      public override bool IsValid => base.IsValid && !string.IsNullOrEmpty(CompanyId);
    };
    #endregion
  }
}
