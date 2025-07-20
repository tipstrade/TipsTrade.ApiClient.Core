namespace TipsTrade.ApiClient.Core.Tenant {
  /// <summary>A collection of methods for the <see cref="Tenant"/> namespace.</summary>
  public static class Extensions {
    /// <summary>Retrieves the tenant, or the default value.</summary>
    /// <typeparam name="T">The type of tenant.</typeparam>
    /// <param name="tenant">The current value.</param>
    /// <param name="defaultValue">The default tenant, if the provider is null.</param>
    /// <returns></returns>
    public static async Task<T> GetTenantOrDefault<T>(this IGetTenant<T>? tenant, T defaultValue) {
      return tenant != null ? await tenant.GetTenantAsync() : defaultValue;
    }

    /// <summary>Gets the tenant, or the default string.</summary>
    /// <param name="tenant">The current value.</param>
    /// <returns>The tenant string, or <c>(default)</c></returns>
    public static Task<string> GetTenantOrDefault(this IGetTenant? tenant) {
      return tenant.GetTenantOrDefault("(default)");
    }
  }
}
