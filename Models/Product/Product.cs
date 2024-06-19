using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web_2.Migrations;

namespace Web_2.Models.Product;

public class Product
{
    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
    public string Image { get; set; }
    public string Decription { get; set; }
    public int  Stockquantity{ get; set; }
    public int Sellerid { get; set; }
    
    public DateTime Daycreated { get; set; }
}
