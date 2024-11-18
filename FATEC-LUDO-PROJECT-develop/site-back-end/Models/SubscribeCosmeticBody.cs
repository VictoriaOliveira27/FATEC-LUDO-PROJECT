using System.Text.Json.Serialization;

public class SubscribeCosmeticBody
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("info")]
    public string Info { get; set; }

    [JsonPropertyName("price")]
    public int Price { get; set; }

    [JsonPropertyName("image")]
    public string Image { get; set; }

    [JsonPropertyName("data")]
    public string Data { get; set; }

    [JsonPropertyName("item_type")]
    public string ItemType { get; set; }
}