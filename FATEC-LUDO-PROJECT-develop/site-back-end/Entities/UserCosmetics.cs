using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("user_cosmetics")]
public class UserCosmetics {
    [Key]
    public string user_id { get; set; }
    public List<string> available_cosmetics { get; set; }
    public List<string> wishlist_cosmetics { get; set;}
    public List<string> applied_cosmetics { get; set; }
}