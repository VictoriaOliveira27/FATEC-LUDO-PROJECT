using System.Text.Json.Serialization;

public class ListUsersBody {
    [JsonPropertyName("page")]
    public int Page { get; set; }
    [JsonPropertyName("items_per_page")]
    public int ItemsPerPage { get; set; }
}