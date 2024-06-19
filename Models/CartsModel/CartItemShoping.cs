using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Web_2.Models.Carts;

public class CartItemShoping
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CartItemId { get; set; }
    public int CartId { get; set; }
    
    [JsonIgnore]
    public CartShoping CartShoping { get; set; }
    public int ProductId { get; set; }
    public Product.Product Product { get; set; }
   
}