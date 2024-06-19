using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_2.Data;
using Web_2.Migrations;
using Web_2.Models.Product;

namespace Web_2.Controllers.ProductController;


[ApiController]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPut]
    [Route("AddProduct")]
    public async Task<IActionResult> AddProduct(Product product)
    {
        await _context.product.AddAsync(product);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet]
    [Route("Find_a_Product")]
    public async Task<IActionResult> FindProduct(int id)
    {
        var findResult = await (from p in _context.product where p.id == id select p).FirstOrDefaultAsync();
        if (findResult == null)
        {
            return NotFound("Khong co hang hoa");
        }

        return Ok(findResult);
    }

    [HttpGet]
    [Route("Find_Product_UserId/{sellerId}")]
    public async Task<IActionResult> FindProductUserid(int sellerId)
    {
        var products = await _context.product.Where(p => p.Sellerid == sellerId).ToListAsync();

        if (products == null)
        {
            return NotFound("Khong co product");
        }

        return Ok(products);
    }

    [HttpGet]
    [Route("Get_All_Produc")]
    public async Task<IActionResult> GetAllProduct(int page = 1, int limit = 4)
    {
        if (page < 1) page = 1;
        if (limit < 1) limit = 4;//giới hạn số lượng mỗi lần lấy prouct là 4 

        var skip = (page - 1) * limit;
        var getallprodut = await _context.product
            .Skip(skip)
            .Take(limit)
            .ToListAsync();
        return Ok(getallprodut);
    }
    [HttpDelete]
    [Route("DeleteProduct")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var findResult = await (from p in _context.product where p.id == id select p).FirstOrDefaultAsync();
        if (findResult == null)
        {
            return NotFound("Khong co hang hoa");
        }

        _context.product.Remove(findResult);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "User deleted successfully" });
    }

    [HttpPatch]
    [Route("ChangeProduct")]
    public async Task<IActionResult> ChangeProduct([FromBody] Product content)
    {
        var findAsync = await (from p in _context.product where p.id == content.id select p).FirstOrDefaultAsync();
        if (findAsync == null)
        {
            return NotFound(new { Message = "Product not found" });
        }
        
        // update Product
        findAsync.Name = content.Name;
        findAsync.Value = content.Value;
        findAsync.Image = content.Image;
        findAsync.Decription = content.Decription;
        findAsync.Stockquantity = content.Stockquantity;
        findAsync.Sellerid = content.Sellerid;
        findAsync.Daycreated = content.Daycreated;
        
        await _context.SaveChangesAsync();
        return Ok(new { Message = "User updated successfully" });
    }
}
