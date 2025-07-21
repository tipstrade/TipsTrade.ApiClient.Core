namespace TipsTrade.ApiClient.Core.Tenant {
  /// <summary>A collection of methods for the <see cref="Tenant"/> namespace.</summary>
  public static class Extensions {
    /// <summary>Retrieves the tenant, or the default value.</summary>
    /// <typeparam name="T">The type of tenant.</typeparam>
    /// <param name="tenant">The current value.</param>
    /// <param name="defaultValue">The default tenant, if the provider is null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The tenant value, or the value in <c><paramref name="defaultValue"/></c>.</returns>
    public static async Task<T> GetTenantOrDefaultAsync<T>(this IGetTenant<T>? tenant, T defaultValue, CancellationToken cancellationToken = default) {
      return tenant != null ? await tenant.GetTenantAsync(cancellationToken) : defaultValue;
    }

    /// <summary>Gets the tenant string, or the default string.</summary>
    /// <param name="tenant">The current value.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The tenant string, or <c>(default)</c></returns>
    public static Task<string> GetTenantOrDefaultAsync(this IGetTenant? tenant, CancellationToken cancellationToken = default) {
      return tenant.GetTenantOrDefaultAsync("(default)", cancellationToken);
    }
  }
}
