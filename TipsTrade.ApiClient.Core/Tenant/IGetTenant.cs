namespace TipsTrade.ApiClient.Core.Tenant {
  /// <summary>Provides methods for retrieving a string tenant.</summary>
  public interface IGetTenant {
    /// <summary>Gets the tenant.</summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task<string> GetTenantAsync(CancellationToken cancellationToken = default);
  }

  /// <summary>Provides methods for retrieving a tenant.</summary>
  /// <typeparam name="T">The type of tenant.</typeparam>
  public interface IGetTenant<T> {
    /// <summary>Gets the tenant.</summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task<T> GetTenantAsync(CancellationToken cancellationToken = default);
  }
}