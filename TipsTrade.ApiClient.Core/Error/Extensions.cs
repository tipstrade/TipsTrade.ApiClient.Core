using System.Net;

namespace TipsTrade.ApiClient.Core.Error {
  /// <summary>Provides extension methods for <see cref="Error"/> namespace.</summary>
  public static class Extensions {
    /// <summary>Returns a human-readable message from the specified <see cref="HttpStatusCode"/>.</summary>
    public static string GetErrorMessage(this HttpStatusCode statusCode) {
      return statusCode switch {
        HttpStatusCode.BadRequest => "The request was invalid or cannot be otherwise served.",
        HttpStatusCode.Unauthorized => "Authentication is required and has failed or has not yet been provided.",
        HttpStatusCode.Forbidden => "The request is understood, but it has been refused or access is not allowed.",
        HttpStatusCode.NotFound => "The requested resource could not be found.",
        HttpStatusCode.MethodNotAllowed => "The request method is not allowed for the specified resource.",
        (HttpStatusCode)429 => "You have sent too many requests in a given amount of time.",
        HttpStatusCode.InternalServerError => "An unexpected condition was encountered on the server.",
        HttpStatusCode.NotImplemented => "The server does not support the functionality required to fulfill the request.",
        HttpStatusCode.BadGateway => "The server received an invalid response from an upstream server.",
        HttpStatusCode.ServiceUnavailable => "The server is currently unable to handle the request due to maintenance or overload.",
        HttpStatusCode.GatewayTimeout => "The server did not receive a timely response from the upstream server.",
        _ => $"The server returned a {statusCode} response."
      };
    }
  }
}
