# TipsTrade.ApiClient.Testing [![NuGet](https://img.shields.io/nuget/v/TipsTrade.ApiClient.Testing)](https://www.nuget.org/packages/TipsTrade.ApiClient.Testing)

Utilities to help unit tests validate JSON attribute usage and other small testing concerns.

Versioning
- As of version 8.x the package major version follows the .NET target version (e.g. `8.x.x` targets `net8.0`).

Supported target framework
- `net8.0`

Intended use
- Project supplies lightweight test helpers (designed to be referenced from unit test projects) that integrate with NUnit's `TestCaseData` to produce parameterized tests.

Namespaces and important types

TipsTrade.ApiClient.Testing
- `JsonAttributeAssert` — static test helper focused on JSON attribute consistency across serializers:
  - Test-case generation:
    - `GetTestCases(this Assembly assembly, Func<Type, bool>? typePredicate = null, Func<PropertyInfo, bool>? propertyPredicate = null)` — enumerate properties from an assembly that have JSON attributes and return `IEnumerable<TestCaseData<PropertyInfo>>`.
    - `GetTestCases<T>(Func<PropertyInfo, bool>? predicate = null)` — properties for a specific type.
    - `GetTestCases(this Type type, Func<PropertyInfo, bool>? predicate = null)` — properties for a given `Type`.
    - `GetTestCases(this IEnumerable<PropertyInfo> properties)` — build `TestCaseData` for an enumerable of properties.
  - Assertions:
    - `IsConsistent(PropertyInfo property)` — asserts both ignore and name attributes are consistent.
    - `IsNameConsistent(PropertyInfo property)` — asserts `Newtonsoft.Json.JsonProperty` and `System.Text.Json.Serialization.JsonPropertyName` (if present) have the same name.
    - `IsIgnoreConsistent(PropertyInfo property)` — asserts `JsonIgnore` attributes are present/absent consistently between Newtonsoft and System.Text.Json.

Behavior details
- The helper looks for both `Newtonsoft.Json` and `System.Text.Json` attributes and yields NUnit `TestCaseData` named using the declaring type and property name.
- Designed to be used from NUnit tests via `TestCaseSource` or by iterating the test cases returned from an assembly/type.

Notes
- This README was updated to reflect the actual public surface found in source (`JsonAttributeAssert`) and the project's target framework (`net8.0`).
- For examples, see the repository's `Tests` project which consumes these helpers to validate JSON attribute consistency across models.
