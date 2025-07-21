using System.Net;
using TipsTrade.ApiClient.Core.Error;

namespace Tests.Errors {
  public class Tests {
    [Test(Description = "ApiException.FromHttpError generates a custom error message.")]
    public void ApiException_From_StatusCode() {
      var ex = ApiException.FromHttpError(HttpStatusCode.NotFound);

      using (Assert.EnterMultipleScope()) {
        Assert.That(ex.Error, Is.Null);
        Assert.That(ex.Message, Is.Not.Null);
        Assert.That(ex.InnerException, Is.Null);
        Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
      }
    }

    [Test(Description = "ApiException.FromHttpError uses the error.")]
    public void ApiException_From_StatusCode_With_Error() {
      var expected = (RequestId: Guid.NewGuid(), Endpoint: "users");
      var ex = ApiException.FromHttpError(HttpStatusCode.NotFound, error: expected);

      Assert.Multiple(() => {
        Assert.That(ex.Error, Is.EqualTo(expected));
        Assert.That(ex.GetError<(Guid RequestId, string Endpoint)>, Is.EqualTo(expected));

        // Default values
        Assert.That(ex.GetError<string>(), Is.Null);
        Assert.That(ex.GetError<int>(), Is.Zero);
      });
    }

    [Test(Description = "ApiException.FromHttpError uses the provided message.")]
    public void ApiException_From_StatusCode_With_Message() {
      var expected = "A custom message.";
      var ex = ApiException.FromHttpError(HttpStatusCode.NotFound, expected);

      Assert.That(ex.Message, Is.EqualTo(expected));
    }

    [Test(Description = "ApiException.FromHttpError uses all the parameters.")]
    public void ApiException_From_StatusCode_With_Parameters() {
      var expectedMessage = "A custom message";
      var expectedStatus = HttpStatusCode.NotFound;
      var expectedError = (RequestId: Guid.NewGuid(), Endpoint: "users");
      var expectedException = new InvalidOperationException();
      var ex = ApiException.FromHttpError(expectedStatus, message: expectedMessage, error: expectedError, innerException: expectedException);

      var error = ex.GetError<(Guid RequestId, string Endpoint)>();

      Assert.Multiple(() => {
        Assert.That(ex.Error, Is.EqualTo(expectedError));
        Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        Assert.That(ex.InnerException, Is.EqualTo(expectedException));
        Assert.That(ex.StatusCode, Is.EqualTo(expectedStatus));
      });
    }

    [Test(Description = "ApiException.FromHttpError uses the provided provider.")]
    public void ApiException_From_StatusCode_With_Provider() {
      var expected = "DVLA MOT";
      var ex = ApiException.FromHttpError(HttpStatusCode.NotFound, provider: expected);

      Assert.That(ex.Provider, Is.EqualTo(expected));
    }

    [Test(Description = "HttpStatusCode.GetErrorMessage extension succeeds.")]
    public void HttpStatusCode_GetErrorMessage() {
      var fallback = @"The server returned a \d+ response";

      Assert.Multiple(() => {
        // 404 has a custom message
        Assert.That(HttpStatusCode.NotFound.GetErrorMessage(), Does.Not.Match(fallback));

        // Fallback 
        Assert.That(((HttpStatusCode)0).GetErrorMessage(), Does.Match(fallback));
      });
    }
  }
}
