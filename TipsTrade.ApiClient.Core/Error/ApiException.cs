using System.Net;

namespace TipsTrade.ApiClient.Core.Error {
  /// <summary>Represents an exception thrown by an API.</summary>
  public class ApiException : Exception {
    #region Properties
    /// <summary>The optional error data.</summary>
    public object? Error { get; set; }

    /// <summary>The optional provider name.</summary>
    public string? Provider { get; set; }

    /// <summary>The optional HTTP status code returned by the API.</summary>
    public HttpStatusCode? StatusCode { get; set; }
    #endregion

    #region Lifecycle
    /// <summary>Creates an instance of the <see cref="ApiException"/> class.</summary>
    public ApiException() { }

    /// <summary>Creates an instance of the <see cref="ApiException"/> class.</summary>
    public ApiException(string message) : base(message) { }

    /// <summary>Creates an instance of the <see cref="ApiException"/> class.</summary>
    public ApiException(string message, Exception? innerException) : base(message, innerException) { }
    #endregion

    #region Methods
    /// <summary>Creates a <see cref="ApiException"/> from the specified <see cref="StatusCode"/>.</summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="message">The optional error message.</param>
    /// <param name="innerException">The optional inner exception that was thrown.</param>
    /// <param name="provider">The optional provider name.</param>
    /// <param name="error">The optional error data.</param>
    public static ApiException FromHttpError(HttpStatusCode statusCode, string? message = null, Exception? innerException = null, string? provider = null, object? error = null) {
      return new ApiException(message ?? statusCode.GetErrorMessage(), innerException) {
        Error = error,
        Provider = provider,
        StatusCode = statusCode,
      };
    }

    /// <summary>Gets the <see cref="Error"/> cast as the specified type.</summary>
    /// <typeparam name="T">The type of error to retrieve.</typeparam>
    /// <returns>The error is cast successfuly, or <c>default(T)</c>.</returns>
    public T? GetError<T>() => Error is T t ? t : default;
    #endregion
  }
}
