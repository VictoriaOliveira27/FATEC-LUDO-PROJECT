using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
[Table("user")]
public class User {
    [Key]
    public string id { get; set; }
    public string username { get; set; }
    public string email { get; set; }  
    public string password { get; set; }
    public int user_level { get; set; }
    public int loses { get; set; }
    public int wins { get; set; }   
    public int ludo_coins { get; set; } 
    public bool is_admin { get; set;}
    public DateTime created_at { get; set; }    
    public DateTime updated_at { get; set; }
}