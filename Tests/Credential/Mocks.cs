using Moq;
using TipsTrade.ApiClient.Core.Credential;

namespace Tests.Credential {
  internal static class Mocks {
    internal static Mock<IGetCredential<string, UserCredential>> GetUserCredentialMock() {
      var mock = new Mock<IGetCredential<string, UserCredential>>();

      mock.Setup(x => x.GetCredentialAsync("mot", default)).ReturnsAsync(new UserCredential("mot-user", "password"));
      mock.Setup(x => x.GetCredentialAsync("enquiry", default)).ReturnsAsync(new UserCredential("enquiry-user", "123456"));
      mock.Setup(x => x.GetCredentialAsync("recall", default)).ReturnsAsync("Not found.");
      mock.Setup(x => x.GetCredentialAsync("unsupported", default)).Throws<NotSupportedException>();

      return mock;
    }

    internal static Mock<IGetGenericCredential<(string, string)>> GetGenericMock() {
      var mock = new Mock<IGetGenericCredential<(string, string)>>();

      mock
        .Setup(x => x.GetCredentialAsync<UserCredential>(new Tuple<string, string>("alice", "Provider1").ToValueTuple(), default))
        .ReturnsAsync(new UserCredential("alice", "123456"));
      mock
        .Setup(x => x.GetCredentialAsync<ApiKeyCredential>(new Tuple<string, string>("bob", "Provider2").ToValueTuple(), default))
        .ReturnsAsync(new ApiKeyCredential("abcdef"));
      mock
        .Setup(x => x.GetCredentialAsync<ApiKeyCredential>(new Tuple<string, string>("eve", "Provider3").ToValueTuple(), default))
        .ReturnsAsync("Eve has been banned.");
      mock
        .Setup(x => x.GetCredentialAsync<DummyCredential>(new Tuple<string, string>("freddie", "Provider3").ToValueTuple(), default))
        .Throws<NotSupportedException>();

      return mock;
    }

    internal static Mock<ISetCredential<string, UserCredential>> SetCredentialMock() {
      var mock = new Mock<ISetCredential<string, UserCredential>>();

      return mock;
    }
  }

  internal class DummyCredential : UserCredential {
    public string CompanyId { get; set; } = "";

    public override bool IsValid => base.IsValid && !string.IsNullOrEmpty(CompanyId);
  };
}
