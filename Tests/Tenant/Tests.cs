using Moq;
using TipsTrade.ApiClient.Core.Tenant;

namespace Tests.Tenant {
  public class Tests {
    private static Mock<IGetTenant<T>> SetupTenantMock<T>(T tennantValue) {
      var mock = new Mock<IGetTenant<T>>();

      mock.Setup(x => x.GetTenantAsync()).ReturnsAsync(tennantValue);

      return mock;
    }

    [Test(Description = "Null GetTenantOrDefault<int> succeeds")]
    public async Task NullGetIntTenantOrDefaultSucceeds() {
      IGetTenant<int>? nullMock = null;
      var expected = -1;

      var actual = await nullMock.GetTenantOrDefault(expected);

      Assert.That(actual, Is.EqualTo(expected));
    }

    [Test(Description = "Null GetTenantOrDefault<string> succeeds")]
    public async Task NullGetStringTenantOrDefaultSucceeds() {
      IGetTenant? nullMock = null;
      var expected = "(default)";
      string actual;

      // With no default
      actual = await nullMock.GetTenantOrDefault();

      Assert.That(actual, Is.EqualTo(expected));

      // With a default value
      expected = "None";
      actual = await nullMock.GetTenantOrDefault("None");

      Assert.That(actual, Is.EqualTo(expected));
    }

    [Test(Description = "IGetTenant succeeds")]
    public async Task GetSucceeds() {
      var expected = "Bob";
      string actual;

      var mock = new Mock<IGetTenant>();
      mock.Setup(x => x.GetTenantAsync()).ReturnsAsync(expected);

      actual = await mock.Object.GetTenantOrDefault();

      Assert.That(actual, Is.EqualTo(expected));
      mock.Verify(x => x.GetTenantAsync());
    }

    [Test(Description = "IGetTenant<int> succeeds")]
    public async Task GetIntSucceeds() {
      var expected = 100;
      int actual;

      var mock = SetupTenantMock(expected);
      actual = await mock.Object.GetTenantOrDefault(-1);

      Assert.That(actual, Is.EqualTo(expected));
      mock.Verify(x => x.GetTenantAsync());
    }

    [Test(Description = "IGetTenant<Tuple> succeeds")]
    public async Task GetTupleSucceeds() {
      var expected = (Name: "bob", Service: "MOT");
      dynamic actual;

      var mock = SetupTenantMock(expected);
      actual = await mock.Object.GetTenantOrDefault((Name: "(default)", Service: "Any"));

      Assert.That(actual, Is.EqualTo(expected));
      mock.Verify(x => x.GetTenantAsync());
    }
  }
}