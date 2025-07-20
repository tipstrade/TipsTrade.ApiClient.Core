namespace TipsTrade.ApiClient.Core.Tenant {
  /// <summary>Provides methods for retrieving a tenant.</summary>
  /// <typeparam name="T">The type of tenant.</typeparam>
  public interface IGetTenant<T> {
    /// <summary>Gets the tenant.</summary>
    Task<T> GetTenantAsync();
  }

  /// <summary>Provides methods for retrieving a tenant.</summary>
  public interface IGetTenant : IGetTenant<string> { }
}
