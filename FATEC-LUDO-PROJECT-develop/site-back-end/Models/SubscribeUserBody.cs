using System.Text.Json;
using System.Text.Json.Serialization;
public class SubscribeUserBody 
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; } 
    
    [JsonPropertyName("password")]
    public string Password { get; set; }
}