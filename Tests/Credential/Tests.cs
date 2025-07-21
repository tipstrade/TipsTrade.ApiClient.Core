using TipsTrade.ApiClient.Core.Credential;

namespace Tests.Credential {
  public class Tests {
    [Test(Description = "CredentialResponse casts from exception.")]
    public void CredentialResponse_Casts_From_Exception() {
      var expected = new InvalidOperationException("The value isn't valid.");

      CredentialResponse<UserCredential> fromError = expected;

      Assert.Multiple(() => {
        Assert.That(fromError, Is.Not.Null);
        Assert.That(fromError.Message, Is.Null);
        Assert.That(fromError.Value, Is.Null);
        Assert.That(fromError.Exception, Is.EqualTo(expected));
        Assert.That(fromError.IsSuccess, Is.False);
      });
    }

    [Test(Description = "CredentialResponse casts from string.")]
    public void CredentialResponse_Casts_From_String() {
      var expected = "Not found.";

      CredentialResponse<UserCredential> fromMessage = expected;

      Assert.Multiple(() => {
        Assert.That(fromMessage, Is.Not.Null);
        Assert.That(fromMessage.Message, Is.EqualTo(expected));
        Assert.That(fromMessage.Value, Is.Null);
        Assert.That(fromMessage.Exception, Is.Null);
        Assert.That(fromMessage.IsSuccess, Is.False);
      });
    }

    [Test(Description = "CredentialResponse casts from string.")]
    public void CredentialResponse_Casts_From_Value() {
      var expected = new UserCredential("bob", "password");
      CredentialResponse<UserCredential> fromUser = expected;

      Assert.Multiple(() => {
        Assert.That(fromUser, Is.Not.Null);
        Assert.That(fromUser.Message, Is.Null);
        Assert.That(fromUser.Value, Is.EqualTo(expected));
        Assert.That(fromUser.Value?.Username, Is.EqualTo("bob"));
        Assert.That(fromUser.Value?.Password, Is.EqualTo("password"));
        Assert.That(fromUser.Exception, Is.Null);
        Assert.That(fromUser.IsSuccess, Is.True);
      });
    }

    [Test(Description = "GetCredential succeeds.")]
    public async Task GetCredential() {
      var mock = Mocks.GetUserCredentialMock();

      var mot = await mock.Object.TryGetCredentialAsync("mot");
      Assert.Multiple(() => {
        Assert.That(mot, Is.Not.Null);
        Assert.That(mot.IsSuccess, Is.True);
        Assert.That(mot.Value?.Username, Is.EqualTo("mot-user"));
        Assert.That(mot.Value?.Password, Is.EqualTo("password"));
        Assert.That(mot.Message, Is.Null);
        Assert.That(mot.Exception, Is.Null);
      });

      var enquiry = await mock.Object.TryGetCredentialAsync("enquiry");
      Assert.Multiple(() => {
        Assert.That(enquiry, Is.Not.Null);
        Assert.That(enquiry.IsSuccess, Is.True);
        Assert.That(enquiry.Value?.Username, Is.EqualTo("enquiry-user"));
        Assert.That(enquiry.Value?.Password, Is.EqualTo("123456"));
        Assert.That(enquiry.Message, Is.Null);
        Assert.That(enquiry.Exception, Is.Null);
      });

      var recall = await mock.Object.TryGetCredentialAsync("recall");
      Assert.Multiple(() => {
        Assert.That(recall, Is.Not.Null);
        Assert.That(recall.IsSuccess, Is.False);
        Assert.That(recall.Value, Is.Null);
        Assert.That(recall.Message, Is.EqualTo("Not found."));
        Assert.That(enquiry.Exception, Is.Null);
      });

      var unsupported = await mock.Object.TryGetCredentialAsync("unsupported");
      Assert.Multiple(() => {
        Assert.That(unsupported, Is.Not.Null);
        Assert.That(unsupported.IsSuccess, Is.False);
        Assert.That(unsupported.Value, Is.Null);
        Assert.That(unsupported.Message, Is.Null);
        Assert.That(unsupported.Exception, Is.InstanceOf<NotSupportedException>());
      });
    }

    [Test(Description = "GetGenericCredential succeeds.")]
    public async Task GetGenericCredential() {
      var mock = Mocks.GetGenericMock();

      var alce = await mock.Object.TryGetCredentialAsync<(string, string), UserCredential>(("alice", "Provider1"));

      Assert.Multiple(() => {
        Assert.That(alce, Is.Not.Null);
        Assert.That(alce.IsSuccess, Is.True);
        Assert.That(alce.Value?.Username, Is.EqualTo("alice"));
        Assert.That(alce.Value?.Password, Is.EqualTo("123456"));
        Assert.That(alce.Message, Is.Null);
        Assert.That(alce.Exception, Is.Null);
      });

      var bob = await mock.Object.TryGetCredentialAsync<(string, string), ApiKeyCredential>(("bob", "Provider2"));
      Assert.Multiple(() => {
        Assert.That(bob, Is.Not.Null);
        Assert.That(bob.IsSuccess, Is.True);
        Assert.That(bob.Value?.ApiKey, Is.EqualTo("abcdef"));
        Assert.That(bob.Message, Is.Null);
        Assert.That(bob.Exception, Is.Null);
      });

      var eve = await mock.Object.TryGetCredentialAsync<(string, string), ApiKeyCredential>(("eve", "Provider3"));
      Assert.Multiple(() => {
        Assert.That(eve, Is.Not.Null);
        Assert.That(eve.IsSuccess, Is.False);
        Assert.That(eve.Value, Is.Null);
        Assert.That(eve.Message, Is.EqualTo("Eve has been banned."));
        Assert.That(bob.Exception, Is.Null);
      });

      var unsupported = await mock.Object.TryGetCredentialAsync<(string, string), DummyCredential>(("freddie", "Provider3"));
      Assert.Multiple(() => {
        Assert.That(unsupported, Is.Not.Null);
        Assert.That(unsupported.IsSuccess, Is.False);
        Assert.That(unsupported.Value, Is.Null);
        Assert.That(unsupported.Message, Is.Null);
        Assert.That(unsupported.Exception, Is.InstanceOf<NotSupportedException>());
      });
    }

    [TestCaseSource(nameof(IsValidFalseCases))]
    public void IIsValid_False(IIsValid value) {
      Assert.That(value.IsValid, Is.False);
    }

    [TestCaseSource(nameof(IsValidTrueCases))]
    public void IIsValid_True(IIsValid value) {
      Assert.That(value.IsValid, Is.True);
    }

    [Test(Description = "SetCredentialAsync succeeds.")]
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
  }
}
