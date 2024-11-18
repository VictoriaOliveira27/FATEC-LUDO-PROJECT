using System.Text.Json.Serialization;

public class ListInventoryBody {
    [JsonPropertyName("user_id")]
    public string UserId { get; set;}

    [JsonPropertyName("page")]
    public int Page { get; set; }
    
    [JsonPropertyName("items_per_page")]
    public int ItemsPerPage { get; set; }
}