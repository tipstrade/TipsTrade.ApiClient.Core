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
      var missingIgnore1 = () => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingIgnore1)));
      var missingIgnore2 = () => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingIgnore2)));
      var notTested = () => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.NotTested)));
      var consistentIgnore = () => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.ConsistentIgnore)));
      var consistentName = () => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.ConsistentName)));
      var inconsistentName = () => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.InconsistentName)));
      var missingName1 = () => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingName1)));
      var missingName2 = () => JsonAttributeAssert.IsIgnoreConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingName2)));

      using (Assert.EnterMultipleScope()) {
        Assert.That(missingIgnore1, Throws.InstanceOf<AssertionException>());
        Assert.That(missingIgnore2, Throws.InstanceOf<AssertionException>());

        Assert.That(notTested, Throws.Nothing);
        Assert.That(consistentIgnore, Throws.Nothing);
        Assert.That(consistentName, Throws.Nothing);
        Assert.That(inconsistentName, Throws.Nothing);
        Assert.That(missingName1, Throws.Nothing);
        Assert.That(missingName2, Throws.Nothing);
      }
    }

    [Test(Description = "IsNameConsistent asserts correctly")]
    public void IsPropertyNameConsistent_Asserts_Correctly() {
      var inconsistentName = () => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.InconsistentName)));
      var missingName1 = () => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingName1)));
      var missingName2 = () => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingName2)));
      var notTested = () => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.NotTested)));
      var consistentIgnore = () => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.ConsistentIgnore)));
      var consistentName = () => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.ConsistentName)));
      var missingIgnore1 = () => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingIgnore1)));
      var missingIgnore2 = () => JsonAttributeAssert.IsNameConsistent(GetProperty<InvalidModel>(nameof(InvalidModel.MissingIgnore2)));

      using (Assert.EnterMultipleScope()) {
        Assert.That(inconsistentName, Throws.InstanceOf<AssertionException>());
        Assert.That(missingName1, Throws.InstanceOf<AssertionException>());
        Assert.That(missingName2, Throws.InstanceOf<AssertionException>());

        Assert.That(notTested, Throws.Nothing);
        Assert.That(consistentIgnore, Throws.Nothing);
        Assert.That(consistentName, Throws.Nothing);
        Assert.That(missingIgnore1, Throws.Nothing);
        Assert.That(missingIgnore2, Throws.Nothing);
      }
    }

    [Test, TestCaseSource(nameof(GetTestData))]
    public void ValidModel_Should_Pass(PropertyInfo property) {
      JsonAttributeAssert.IsConsistent(property);
    }

    private PropertyInfo GetProperty<T>(string name) => typeof(T).GetProperty(name) ?? throw new ArgumentException($"{name} doesn't exist on {typeof(T)}.");

    internal static IEnumerable<TestCaseData<PropertyInfo>> GetTestData() => JsonAttributeAssert.GetTestCases<ValidModel>();
  }
}