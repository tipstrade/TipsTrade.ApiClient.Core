# TipsTrade.ApiClient.Core [![NuGet](https://img.shields.io/nuget/v/TipsTrade.ApiClient.Core)](https://www.nuget.org/packages/TipsTrade.ApiClient.Core)

A collection of interfaces, classes and extension methods commonly used when writing API client libraries.

Versioning
- As of version 8.x the package major version follows the .NET target version (e.g. `8.x.x` targets `net8.0`).

Supported target frameworks
- `net8.0`
- `net481` (for compatibility with older .NET Framework projects, but may not receive all new features going forward).

Packaging
- Project generates a NuGet package on build (`GeneratePackageOnBuild=true`) and uses `README.md` as the package readme.
- Project depends on `Microsoft.Extensions.Logging.Abstractions` (core package).

Namespaces and important types

TipsTrade.ApiClient.Core.Caching
- `IAddToCache<TKey>` / `IAddToCache<TKey, TValue>` — methods to add items to a cache (`AddToCacheAsync`).
- `IGetFromCache<TKey>` / `IGetFromCache<TKey, TValue>` — methods to retrieve items from a cache (`GetFromCacheAsync`).
- `IReadWriteCache<TKey>` / `IReadWriteCache<TKey, TValue>` — convenience interfaces combining add/get.

TipsTrade.ApiClient.Core.Credential
- `IIsValid` — exposes `bool IsValid { get; }` for credential validation.
- `UserCredential` — simple user credentials (`Username`, `Password`) with `IsValid` behavior.
- `ApiKeyCredential` — simple API key credentials (`ApiKey`, optional `Secret`) with `IsValid` behavior.
- `IGetCredential<TKey>` and `IGetCredential<TKey, TCredential>` — retrieval methods: `GetCredentialAsync<TCredential>(TKey key, ...)` and `GetCredentialAsync(TKey key, ...)`.
- `ISetCredential<TKey, TCredential>` — `SetCredentialAsync(TKey key, TCredential credential, ...)`.
- Extension methods:
  - `GetCredentialOrThrowAsync` — wraps `GetCredentialAsync` and throws a standardized exception (logs failure).
  - `TrySetCredentialAsync` — calls `SetCredentialAsync` and returns `true` on success, `false` on failure (logs failure).

TipsTrade.ApiClient.Core.Error
- `ApiException` — exception type representing API errors. Properties: `object? Error`, `string? Provider`, `HttpStatusCode? StatusCode`. Factory: `ApiException.FromHttpError(...)`.
- `HttpStatusCode.GetErrorMessage()` — extension that returns a human-readable fallback message for common HTTP status codes.

TipsTrade.ApiClient.Core.Logging
- `IWithLogger` — exposes `ILogger? Logger { get; }`.
- Extensions:
  - `GetLogger<T>(this T? value)` — returns `ILogger?` if the value implements `IWithLogger`.
  - `LogIf(this ILogger? logger, LogLevel level, Func<string> message)` — logs only when the `LogLevel` is enabled.

TipsTrade.ApiClient.Core.Tenant
- `IGetTenant` / `IGetTenant<T>` — `GetTenantAsync(...)` to obtain tenant values.
- Extension methods:
  - `GetTenantOrDefaultAsync(this IGetTenant? tenant)` — returns tenant string or `"(default)"`.
  - `GetTenantOrDefaultAsync<T>(this IGetTenant<T>? tenant, T defaultValue)` — returns tenant or provided default.
  - `GetTenantOrThrowAsync` variants — wrap retrieval, log on error and throw standardized exception.

TipsTrade.ApiClient.Core.Threading
- `KeyedSemaphoreSlim<K>` — a thread-safe keyed `SemaphoreSlim` store backed by `ConcurrentDictionary<K, SemaphoreSlim>`. Features:
  - `WaitAsync(K key, ...)` to wait on a specific key.
  - `Release(K key)` / `TryRelease(K key)` to release.
  - `ContainsKey`, `GetCurrentCount`, and configurable default counts.

Tests
- The repository includes unit tests that exercise credential helpers, error helpers, JSON attribute assertions, threading utilities, and other behaviors.

Notes
- The README has been synchronized with the codebase: method names, available extension methods, and interfaces reflect the actual source files.
- For implementation details, public APIs and XML documentation are available in the source under the respective folders (e.g. `Credential`, `Error`, `Logging`, `Tenant`, `Caching`, `Threading`).
