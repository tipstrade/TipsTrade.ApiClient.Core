namespace Tests.JsonAttributes {
  internal class InvalidModel {
    public string? NotTested { get; set; }

    [Newtonsoft.Json.JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
    public string? ConsistentIgnore { get; set; }

    [Newtonsoft.Json.JsonProperty("Foo"), System.Text.Json.Serialization.JsonPropertyName("Foo")]
    public string? ConsistentName { get; set; }

    [Newtonsoft.Json.JsonProperty("Foo"), System.Text.Json.Serialization.JsonPropertyName("Bar")]
    public string? InconsistentName { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    public string? MissingIgnore1 { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public string? MissingIgnore2 { get; set; }

    [Newtonsoft.Json.JsonProperty("Foo")]
    public string? MissingName1 { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("Foo")]
    public string? MissingName2 { get; set; }
  }

  internal class ValidModel {
    public string? NotTested { get; set; }

    [Newtonsoft.Json.JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
    public string? Ignored { get; set; }

    [Newtonsoft.Json.JsonProperty("Foo"), System.Text.Json.Serialization.JsonPropertyName("Foo")]
    public string? Foo { get; set; }
  }
}
