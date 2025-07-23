using System.Reflection;
using TipsTrade.ApiClient.Testing;

namespace Tests.JsonAttributes {
  public class Tests {
    [Test(Description = "GetTestCases for assembly returns expected cases")]
    public void GetTestCases_For_Assembly_Returns_Expected() {
      var cases = Assembly.GetExecutingAssembly().GetTestCases(
        t => t.FullName?.StartsWith("Tests.JsonAttributes") == true
        );

      Assert.That(cases.Count(), Is.EqualTo(9));
    }

    [Test(Description = "GetTestCases should not include NotTested")]
    public void GetTestCases_Should_Not_Include() {
      var properties = JsonAttributeAssert
        .GetTestCases(typeof(InvalidModel))
        .Select(x => x.Arguments.First())
        .OfType<PropertyInfo>()
        .Select(x => x.Name)
        ;

      Assert.That(properties, Does.Not.Contain(nameof(InvalidModel.NotTested)));
    }

    [Test(Description = "GetTestCases returns expected cases")]
    public void GetTestCases_Returns_Expected() {
      var properties = JsonAttributeAssert
        .GetTestCases(typeof(InvalidModel))
        ;

      Assert.That(properties.Count(), Is.EqualTo(7));
    }

    [Test(Description = "IsIgnoreConsistent asserts correctly")]
    public void IsIgnoreConsistent_Asserts_Correctly() {
      Assert.Throws<AssertionException>(() => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingIgnore1))));
      Assert.Throws<AssertionException>(() => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingIgnore2))));

      Assert.DoesNotThrow(() => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.NotTested))));
      Assert.DoesNotThrow(() => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.ConsistentIgnore))));
      Assert.DoesNotThrow(() => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.ConsistentName))));
      Assert.DoesNotThrow(() => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.InconsistentName))));
      Assert.DoesNotThrow(() => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingName1))));
      Assert.DoesNotThrow(() => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingName2))));
    }

    [Test(Description = "IsNameConsistent asserts correctly")]
    public void IsPropertyNameConsistent_Asserts_Correctly() {
      Assert.Throws<AssertionException>(() => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.InconsistentName))));
      Assert.Throws<AssertionException>(() => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingName1))));
      Assert.Throws<AssertionException>(() => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingName2))));

      Assert.DoesNotThrow(() => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.NotTested))));
      Assert.DoesNotThrow(() => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.ConsistentIgnore))));
      Assert.DoesNotThrow(() => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.ConsistentName))));
      Assert.DoesNotThrow(() => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingIgnore1))));
      Assert.DoesNotThrow(() => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingIgnore2))));
    }

    [Test, TestCaseSource(nameof(GetTestData))]
    public void ValidModel_Should_Pass(PropertyInfo property) {
      JsonAttributeAssert.IsConsistent(property);
    }

    private PropertyInfo GetProperty<T>(string name) => typeof(T).GetProperty(name) ?? throw new ArgumentException($"{name} doesn't exist on {typeof(T)}.");

    internal static IEnumerable<TestCaseData<PropertyInfo>> GetTestData() => JsonAttributeAssert.GetTestCases<ValidModel>();
  }
}