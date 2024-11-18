using System.Text.Json.Serialization;

public class BuyCosmeticBody 
{
    [JsonPropertyName("user_id")]
    public string Userid { get; set; }

    [JsonPropertyName("cosmetic_id")]
    public string CosmeticId { get; set; }
}