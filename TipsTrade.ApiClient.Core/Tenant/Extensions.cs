using Microsoft.Extensions.Logging;
using TipsTrade.ApiClient.Core.Error;
using TipsTrade.ApiClient.Core.Logging;

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
    public static async Task<string> GetTenantOrDefaultAsync(this IGetTenant? tenant, CancellationToken cancellationToken = default) {
      return tenant != null ? await tenant.GetTenantAsync(cancellationToken) : "(default)";
    }

    /// <summary>
    /// Retrieves the tenant or throws an exception if retrieval fails.
    /// </summary>
    /// <typeparam name="T">The type of tenant.</typeparam>
    /// <param name="tenant">The tenant provider instance.</param>
    /// <param name="defaultValue">The default tenant value to use if the provider is null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, containing the tenant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when tenant retrieval fails.</exception>
    public static async Task<T> GetTenantOrThrowAsync<T>(this IGetTenant<T>? tenant, T defaultValue, CancellationToken cancellationToken = default) {
      try {
        return await tenant.GetTenantOrDefaultAsync(defaultValue, cancellationToken);
      } catch (Exception ex) {
        tenant.GetLogger()?.LogError("Failed to retrieve tenant: {message}", ex.Message);
        throw ExceptionFactory.GetTenantFailed(ex);
      }
    }

    /// <summary>
    /// Retrieves the tenant as a string or throws an exception if retrieval fails.
    /// </summary>
    /// <param name="tenant">The tenant provider instance.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, containing the tenant as a string.</returns>
    /// <exception cref="InvalidOperationException">Thrown when tenant retrieval fails.</exception>
    public static async Task<string> GetTenantOrThrowAsync(this IGetTenant? tenant, CancellationToken cancellationToken = default) {
      try {
        return await tenant.GetTenantOrDefaultAsync(cancellationToken);
      } catch (Exception ex) {
        tenant.GetLogger()?.LogError("Failed to retrieve tenant: {message}", ex.Message);
        throw ExceptionFactory.GetTenantFailed(ex);
      }
    }
  }
}