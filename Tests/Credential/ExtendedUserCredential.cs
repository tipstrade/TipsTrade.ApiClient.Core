using TipsTrade.ApiClient.Core.Credential;

namespace Tests.Credential {

  internal class ExtendedUserCredential : UserCredential {
    public string CompanyId { get; set; } = "";

    public new bool IsValid => base.IsValid && !string.IsNullOrEmpty(CompanyId);
  }
}
