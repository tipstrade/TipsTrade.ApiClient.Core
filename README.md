# TipsTrade.ApiClient.Core

A collection of interfaces, classes and extension methods that are commonly used when writing API client libraries.

**Supported versions**
- .Net 8
- .Net Framework 4.8.1

## Namespaces

### TipsTrade.ApiClient.Core.Credential
- `IIsValid`, provides a property for indicating whether a instance is valid.
- `CredentialResponse<T>`, provides properties for returning a credential.
- `IGetCredential`, `IGetCredential<TKey, T>`, `IGetGenericCredential<TKey>`, provides methods for retrieving a credential using a key.
- `ISetCredential<TKey, T>`, provides methods setting a credential using a key.
- `TryGetCredentialAsync` extension method for safely retrieving a credential.
- `TrySetCredentialAsync` extension method for safely setting a credential.

### TipsTrade.ApiClient.Core.Error
- `ApiException`, provides properties for HTTP status codes, data and providers.
- `GetErrorMessage` for retrieving a human-readable message from a `HttpStatusCode`.

### TipsTrade.ApiClient.Core.Tenant
- `IGetTenant`, provides a method for retriving a tenant.
- `GetTenantOrDefault` extension methods for retrieving a tenant.

### TipsTrade.ApiClient.Core.Threading
- `KeyedSemaphoreSlim<K>`, provides a thread-safe keyed `SempahoreSlim`, backed by a `ConcurrentDictionary<K, SemaphoreSlim>`.
