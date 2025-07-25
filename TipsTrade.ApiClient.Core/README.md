# TipsTrade.ApiClient.Core

A collection of interfaces, classes and extension methods that are commonly used when writing API client libraries.

**Supported versions**
- .Net 8
- .Net Framework 4.8.1

## Namespaces

### TipsTrade.ApiClient.Core.Caching
- `IAddToCache<TKey, TValue>`, provides a method for adding items to a cache.
- `IGetFromCache<TKey, TValue>`, provides a method for getting items from the cache.
- `IReadWriteCache<TKey, TValue>`, provides methods for getting and setting items in the cache.

### TipsTrade.ApiClient.Core.Credential
- `IIsValid`, provides a property for indicating whether a instance is valid.
- `UserCredential`, `ApiKeyCredential`, provides properties for commonly used credential types.
- `IGetCredential<TKey>`, `IGetCredential<TKey, TCredential>`, provides methods for retrieving a credential using a key.
- `ISetCredential<TKey, TCredential>`, provides methods setting a credential using a key.
- `TrySetCredentialAsync` extension method for safely setting a credential.

### TipsTrade.ApiClient.Core.Error
- `ApiException`, provides properties for HTTP status codes, data and providers.
- `GetErrorMessage` for retrieving a human-readable message from a `HttpStatusCode`.

### TipsTrade.ApiClient.Core.Logging
- `IWithLogger`,provides a property that returns an `ILogger`.
- Extension methods for `IWithLogger` and `ILogger`.

### TipsTrade.ApiClient.Core.Tenant
- `GetTenantAsync`, provides a method for retriving a tenant.
- `GetTenantOrDefaultAsync` extension methods for retrieving a tenant.

### TipsTrade.ApiClient.Core.Threading
- `KeyedSemaphoreSlim<K>`, provides a thread-safe keyed `SempahoreSlim`, backed by a `ConcurrentDictionary<K, SemaphoreSlim>`.
