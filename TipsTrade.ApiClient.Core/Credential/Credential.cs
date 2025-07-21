namespace TipsTrade.ApiClient.Core.Credential {
  /// <summary>Provides properties for validating a credential.</summary>
  public interface IIsValid {
    /// <summary>Tests whether the credential is valid.</summary>
    bool IsValid { get; }
  }

  /// <summary>Represents a simple user credential.</summary>
  public class UserCredential : IIsValid {
    /// <summary>The username.</summary>
    public string Username { get; set; } = "";

    /// <summary>The password.</summary>
    public string Password { get; set; } = "";

    /// <summary>Tests whether the credential is valid.</summary>
    public virtual bool IsValid => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);

    /// <summary>Creates an instance of the <see cref="UserCredential"/> class.</summary>
    public UserCredential() { }

    /// <summary>Creates an instance of the <see cref="UserCredential"/> class.</summary>
    public UserCredential(string username, string password) {
      Username = username;
      Password = password;
    }
  }

  /// <summary>Represents a simple API key credential.</summary>
  public class ApiKeyCredential : IIsValid {
    /// <summary>The API key</summary>
    public string ApiKey { get; set; } = "";

    /// <summary>The optional secret.</summary>
    public string? Secret { get; set; }

    /// <summary>Tests whether the credential is valid.</summary>
    public virtual bool IsValid => !string.IsNullOrEmpty(ApiKey);

    /// <summary>Creates an instance of the <see cref="ApiKeyCredential"/> class.</summary>
    public ApiKeyCredential() { }

    /// <summary>Creates an instance of the <see cref="ApiKeyCredential"/> class.</summary>
    public ApiKeyCredential(string apiKey, string? secret = null) {
      ApiKey = apiKey;
      Secret = secret;
    }
  }
}
