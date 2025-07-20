namespace TipsTrade.ApiClient.Core.Credential {
  /// <summary>Represents the response when retrieving a credential.</summary>
  /// <typeparam name="T">The type of credential.</typeparam>
  public class CredentialResponse<T> where T : class, new() {
    /// <summary>The error message.</summary>
    public string? Message { get; set; }

    /// <summary>The inner exception.</summary>
    public Exception? Exception { get; set; }

    /// <summary>A flag indicating whether the response is successful.</summary>
    public bool IsSuccess => Value != null;

    /// <summary>The credential value.</summary>
    public T? Value { get; set; }

    /// <summary>Creates a success response.</summary>
    /// <param name="value">The credential value.</param>
    public static CredentialResponse<T> Success(T value) {
      return new CredentialResponse<T> { Value = value };
    }

    /// <summary>Creates an error response.</summary>
    /// <param name="exception">The exception.</param>
    public static CredentialResponse<T> Error(Exception exception) => new() { Exception = exception };

    /// <summary>Creates a success response.</summary>
    /// <param name="message">The error message.</param>
    /// <param name="exception">The exception.</param>
    public static CredentialResponse<T> Error(string message, Exception? exception = null) => new() { Exception = exception, Message = message };

    /// <summary>Implicitly converts a credential value to a <see cref="CredentialResponse{T}"/>.</summary>
    public static implicit operator CredentialResponse<T>(T value) => CredentialResponse<T>.Success(value);

    /// <summary>Implicitly converts an <see cref="Exception"/> to a <see cref="CredentialResponse{T}"/>.</summary>
    public static implicit operator CredentialResponse<T>(Exception exception) => CredentialResponse<T>.Error(exception);

    /// <summary>Implicitly converts an error string to a <see cref="CredentialResponse{T}"/>.</summary>
    public static implicit operator CredentialResponse<T>(string message) => CredentialResponse<T>.Error(message);

    public override string ToString() {
      return IsSuccess ?
        $"Success {Value}" :
        $"{Message ?? Exception?.Message ?? "Unknown error"}";
    }
  }
}
