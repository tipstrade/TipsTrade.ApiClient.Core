using TipsTrade.ApiClient.Core.Error;

namespace Tests.Errors {
  public class Factory {
    [TestCaseSource(nameof(GetTestCases))]
    public void ExceptionFactory_Succeeds(Exception ex, Exception expectedException, IDictionary<object, object> data) {
      Assert.That(ex, Is.InstanceOf<InvalidOperationException>());
      Assert.That(ex.Message, Is.Not.Empty);
      Assert.That(ex.InnerException, Is.EqualTo(expectedException));
      Assert.That(ex.Data, Is.EqualTo(data));
    }

    internal static IEnumerable<TestCaseData> GetTestCases() {
      var expectedException = new Exception();

      yield return new TestCaseData(
        ExceptionFactory.AddToCacheFailed("bob", 1234, expectedException),
        expectedException,
        new Dictionary<object, object> { { "key", "bob" }, { "item", 1234 } }
        ).SetName("AddToCacheFailed");

      yield return new TestCaseData(
        ExceptionFactory.GetCredentialFailed("bob", expectedException),
        expectedException,
        new Dictionary<object, object> { { "key", "bob" } }
        ).SetName("GetCredentialFailed");

      yield return new TestCaseData(
        ExceptionFactory.GetFromCacheFailed("bob", expectedException),
        expectedException,
        new Dictionary<object, object> { { "key", "bob" } }
        ).SetName("GetFromCacheFailed");

      yield return new TestCaseData(
        ExceptionFactory.GetTenantFailed(expectedException),
        expectedException,
        new Dictionary<object, object> { }
        ).SetName("GetTenantFailed");

      yield return new TestCaseData(
        ExceptionFactory.SetCredentialFailed("bob", 1234, expectedException),
        expectedException,
        new Dictionary<object, object> { { "key", "bob" }, { "credential", 1234 } }
        ).SetName("SetCredentialFailed");
    }
  }
}
