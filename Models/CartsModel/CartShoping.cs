using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Web_2.Models.Carts;

public class CartShoping
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CartId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    [JsonIgnore]
    public List<CartItemShoping> CartItem { get; set; } = new List<CartItemShoping>();
}