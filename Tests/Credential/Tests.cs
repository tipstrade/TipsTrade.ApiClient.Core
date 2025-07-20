using TipsTrade.ApiClient.Core.Credential;

namespace Tests.Credential {
  public class Tests {
    [Test]
    public void CredentialResponseCasts() {
      var ex = new InvalidOperationException("The value isn't valid.");
      var message = "Not found.";

      CredentialResponse<UserCredential> fromError = ex;
      Assert.That(fromError, Is.Not.Null);
      Assert.That(fromError.Message, Is.Null);
      Assert.That(fromError.Value, Is.Null);
      Assert.That(fromError.Exception, Is.EqualTo(ex));
      Assert.That(fromError.IsSuccess, Is.False);

      CredentialResponse<UserCredential> fromMessage = message;
      Assert.That(fromMessage, Is.Not.Null);
      Assert.That(fromMessage.Message, Is.EqualTo(message));
      Assert.That(fromMessage.Value, Is.Null);
      Assert.That(fromMessage.Exception, Is.Null);
      Assert.That(fromMessage.IsSuccess, Is.False);

      CredentialResponse<UserCredential> fromUser = new UserCredential("bob", "password");
      Assert.That(fromUser, Is.Not.Null);
      Assert.That(fromUser.Message, Is.Null);
      Assert.That(fromUser.Value, Is.Not.Null);
      Assert.That(fromUser.Value.Username, Is.EqualTo("bob"));
      Assert.That(fromUser.Value.Password, Is.EqualTo("password"));
      Assert.That(fromUser.Exception, Is.Null);
      Assert.That(fromUser.IsSuccess, Is.True);
    }

    [Test]
    public async Task GetCredential() {
      var mock = Mocks.GetUserCredentialMock();

      var mot = await mock.Object.TryGetCredentialAsync("mot");
      Assert.That(mot, Is.Not.Null);
      Assert.Multiple(() => {
        Assert.That(mot.IsSuccess, Is.True);
        Assert.That(mot.Value?.Username, Is.EqualTo("mot-user"));
        Assert.That(mot.Value?.Password, Is.EqualTo("password"));
        Assert.That(mot.Message, Is.Null);
        Assert.That(mot.Exception, Is.Null);
      });

      var enquiry = await mock.Object.TryGetCredentialAsync("enquiry");
      Assert.That(enquiry, Is.Not.Null);
      Assert.Multiple(() => {
        Assert.That(enquiry.IsSuccess, Is.True);
        Assert.That(enquiry.Value?.Username, Is.EqualTo("enquiry-user"));
        Assert.That(enquiry.Value?.Password, Is.EqualTo("123456"));
        Assert.That(enquiry.Message, Is.Null);
        Assert.That(enquiry.Exception, Is.Null);
      });

      var recall = await mock.Object.TryGetCredentialAsync("recall");
      Assert.That(recall, Is.Not.Null);
      Assert.Multiple(() => {
        Assert.That(recall.IsSuccess, Is.False);
        Assert.That(recall.Value, Is.Null);
        Assert.That(recall.Message, Is.EqualTo("Not found."));
        Assert.That(enquiry.Exception, Is.Null);
      });

      var unsupported = await mock.Object.TryGetCredentialAsync("unsupported");
      Assert.That(unsupported, Is.Not.Null);
      Assert.Multiple(() => {
        Assert.That(unsupported.IsSuccess, Is.False);
        Assert.That(unsupported.Value, Is.Null);
        Assert.That(unsupported.Message, Is.Null);
        Assert.That(unsupported.Exception, Is.InstanceOf<NotSupportedException>());
      });
    }

    [Test]
    public async Task GetGenericCredential() {
      var mock = Mocks.GetGenericMock();

      var alce = await mock.Object.TryGetCredentialAsync<(string, string), UserCredential>(("alice", "Provider1"));
      Assert.That(alce, Is.Not.Null);
      Assert.Multiple(() => {
        Assert.That(alce.IsSuccess, Is.True);
        Assert.That(alce.Value?.Username, Is.EqualTo("alice"));
        Assert.That(alce.Value?.Password, Is.EqualTo("123456"));
        Assert.That(alce.Message, Is.Null);
        Assert.That(alce.Exception, Is.Null);
      });

      var bob = await mock.Object.TryGetCredentialAsync<(string, string), ApiKeyCredential>(("bob", "Provider2"));
      Assert.That(bob, Is.Not.Null);
      Assert.Multiple(() => {
        Assert.That(bob.IsSuccess, Is.True);
        Assert.That(bob.Value?.ApiKey, Is.EqualTo("abcdef"));
        Assert.That(bob.Message, Is.Null);
        Assert.That(bob.Exception, Is.Null);
      });

      var eve = await mock.Object.TryGetCredentialAsync<(string, string), ApiKeyCredential>(("eve", "Provider3"));
      Assert.That(eve, Is.Not.Null);
      Assert.Multiple(() => {
        Assert.That(eve.IsSuccess, Is.False);
        Assert.That(eve.Value, Is.Null);
        Assert.That(eve.Message, Is.EqualTo("Eve has been banned."));
        Assert.That(bob.Exception, Is.Null);
      });

      var unsupported = await mock.Object.TryGetCredentialAsync<(string, string), DummyCredential>(("freddie", "Provider3"));
      Assert.That(unsupported, Is.Not.Null);
      Assert.Multiple(() => {
        Assert.That(unsupported.IsSuccess, Is.False);
        Assert.That(unsupported.Value, Is.Null);
        Assert.That(unsupported.Message, Is.Null);
        Assert.That(unsupported.Exception, Is.InstanceOf<NotSupportedException>());
      });
    }

    [TestCaseSource(nameof(IsValidFalseCases))]
    public void IIsValidFalse(IIsValid value) {
      Assert.That(value.IsValid, Is.False);
    }

    [TestCaseSource(nameof(IsValidTrueCases))]
    public void IIsValidTrue(IIsValid value) {
      Assert.That(value.IsValid, Is.True);
    }

    [Test]
    public async Task SetCredential() {
      var mock = Mocks.SetCredentialMock();
      var key = $"{Guid.NewGuid()}";
      var credential = new UserCredential("bob", "secret");
      var ct = new CancellationTokenSource().Token;

      await mock.Object.TrySetCredentialAsync(key, credential, ct);
      mock.Verify(x => x.SetCredentialAsync(key, credential, ct));

      mock.Invocations.Clear();
      await mock.Object.TrySetCredentialAsync(key, credential);
      mock.Verify(x => x.SetCredentialAsync(key, credential, default));
    }

    internal static IEnumerable<IIsValid> IsValidFalseCases() {
      yield return new ApiKeyCredential();
      yield return new ApiKeyCredential("", "secret");
      yield return new ApiKeyCredential(null!, "secret");
      yield return new UserCredential();
      yield return new UserCredential("", "password");
      yield return new UserCredential(null!, "password");
      yield return new UserCredential("bob", "");
      yield return new UserCredential("bob", null!);
      // Overriden IsValid
      yield return new DummyCredential { Username = "", Password = "password", CompanyId = "a-1234" };
      yield return new DummyCredential { Username = "alice", Password = "", CompanyId = "a-1234" };
      yield return new DummyCredential { Username = "alice", Password = "password", CompanyId = "" };
    }

    internal static IEnumerable<IIsValid> IsValidTrueCases() {
      yield return new ApiKeyCredential("abcdef", "");
      yield return new ApiKeyCredential("abcdef", null!);
      yield return new ApiKeyCredential("abcdef", "secret");
      yield return new UserCredential("bob", "password");
      // Overriden IsValid
      yield return new DummyCredential { Username = "alice", Password = "password", CompanyId = "a-1234" };
    }
  }
}
