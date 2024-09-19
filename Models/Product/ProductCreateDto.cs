namespace Web_2.Models.Product;

public class ProductCreateDto
{
    public string name { get; set; }
    public int value { get; set; }
    public IFormFile image { get; set; }
    public string decription { get; set; }
    public int stockquantity { get; set; }
    public int sellerid { get; set; }
    // public DateTime daycreated { get; set; }
}