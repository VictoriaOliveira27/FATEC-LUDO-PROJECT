using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("cosmetic")]
public class Cosmetic 
{
    [Key]
    public string id { get; set; }
    public string name { get; set;}
    public string info { get; set; }
    public int price { get; set; }
    public string image { get; set; }
    public string data { get; set; }
    public string item_type { get; set;}
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    
}