using NUnit.Framework;
using System.Reflection;

namespace TipsTrade.ApiClient.Testing {
  /// <summary>
  /// The JsonAttributeAssert class contains a collection of static methods that implement Json attribute consistency.
  /// </summary>
  public static class JsonAttributeAssert {
    /// <summary>Get the test cases for the specified type.</summary>
    /// <param name="assembly">The assembly from where to fetch the types.</param>
    /// <param name="typePredicate">An optional predicate for filtering the types.</param>
    /// <param name="propertyPredicate">An optional predicate for filtering the properties.</param>
    public static IEnumerable<TestCaseData<PropertyInfo>> GetTestCases(this Assembly assembly, Func<Type, bool>? typePredicate = null, Func<PropertyInfo, bool>? propertyPredicate = null) {
      var types = assembly.GetTypes().AsEnumerable();

      if (typePredicate != null) {
        types = types.Where(typePredicate);
      }

      var properties = types.SelectMany(type => {
        var props = type.GetProperties().AsEnumerable();

        if (propertyPredicate != null) {
          props = props.Where(propertyPredicate);
        }

        return props;
      });

      return properties.GetTestCases();
    }

    /// <summary>Get the test cases for the specified type.</summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="predicate">An optional predicate for filtering the type's properties.</param>
    public static IEnumerable<TestCaseData<PropertyInfo>> GetTestCases<T>(Func<PropertyInfo, bool>? predicate = null) => typeof(T).GetTestCases(predicate);

    /// <summary>Get the test cases for the specified type.</summary>
    /// <param name="type">The current value.</param>
    /// <param name="predicate">An optional predicate for filtering the type's properties.</param>
    public static IEnumerable<TestCaseData<PropertyInfo>> GetTestCases(this Type type, Func<PropertyInfo, bool>? predicate = null) {
      var properties = type.GetProperties().AsEnumerable();

      if (predicate != null) {
        properties = properties.Where(predicate);
      }

      return properties.GetTestCases();
    }

    /// <summary>Get the test cases for the specified properties.</summary>
    /// <param name="properties">The current collection of properties.</param>
    public static IEnumerable<TestCaseData<PropertyInfo>> GetTestCases(this IEnumerable<PropertyInfo> properties) {
      foreach (var property in properties) {
        var hasJson = property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>() != null
          || property.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>() != null
          || property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>() != null
          || property.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() != null
          ;

        if (hasJson) {
          var testCase = new TestCaseData<PropertyInfo>(property);

          testCase.SetName($"{property.DeclaringType?.Name}_{property.Name}");

          yield return testCase;
        }
      }
    }

    /// <summary>Verifies that the JSON attributes are consistent.</summary>
    /// <param name="property">The <see cref="PropertyInfo"/>.</param>
    public static void IsConsistent(PropertyInfo property) {
      IsIgnoreConsistent(property);
      IsNameConsistent(property);
    }

    /// <summary>Verifies that the JSON property name attributes are consistent.</summary>
    /// <param name="property">The <see cref="PropertyInfo"/>.</param>
    public static void IsNameConsistent(PropertyInfo property) {
      var nsAttr = property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>();
      var stAttr = property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>();

      Assert.That(nsAttr == null, Is.EqualTo(stAttr == null), $"{property.DeclaringType}.{property.Name}: Inconsistent name attributes.");

      if (nsAttr != null || stAttr != null) {
        Assert.That(nsAttr?.PropertyName, Is.EqualTo(stAttr?.Name), $"{property.DeclaringType}.{property.Name}: Inconsistent name.");
      }
    }

    /// <summary>Verifies that the JSON ignore attributes are consistent.</summary>
    /// <param name="property">The <see cref="PropertyInfo"/>.</param>
    public static void IsIgnoreConsistent(PropertyInfo property) {
      var nsAttr = property.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>();
      var stAttr = property.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>();

      if (nsAttr != null || stAttr != null) {
        Assert.That(nsAttr == null, Is.EqualTo(stAttr == null), $"{property.DeclaringType}.{property.Name}: Inconsistent ignore attributes.");
      }
    }
  }
}
