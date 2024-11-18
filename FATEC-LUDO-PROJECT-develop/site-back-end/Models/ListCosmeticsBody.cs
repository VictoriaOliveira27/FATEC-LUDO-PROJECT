using System.Text.Json.Serialization;

public class ListCosmeticsBody 
{
    [JsonPropertyName("page")]
    public int Page { get; set; }
    [JsonPropertyName("items_per_page")]
    public int ItemsPerPage { get; set; }
}