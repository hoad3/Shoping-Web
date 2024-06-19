using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_2.Data;
using Web_2.Models;
using Web_2.Models.Carts;

namespace Web_2.Controllers;


[ApiController]
public class CartsController: ControllerBase
{
    private readonly AppDbContext _context;

    public CartsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("AddCart")]
    public async Task<ActionResult<CartShoping>> AddCart([FromBody] CartShoping cartShoping)
    {
        // Kiểm tra xem UserId đã có CartId chưa
        var existingCart = await _context.CartShoping
            .FirstOrDefaultAsync(c => c.UserId == cartShoping.UserId);
    
        if (existingCart != null)
        {
            return BadRequest("A cart for this user already exists.");
        }

        _context.CartShoping.Add(cartShoping);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCartsById), new { cartsId = cartShoping.CartId }, cartShoping);
    }
    
    [HttpPost("AddCartItem")]
    public async Task<ActionResult<CartItemShoping>> AddCartItem([FromBody] CartItemShopingDto _cartItemShopingDto)
    {
        var cart = await _context.CartShoping.FindAsync(_cartItemShopingDto.CartId);
        if (cart == null)
        {
            return NotFound(new { Message = "Cart not found" });
        }
        // Tạo đối tượng CartItemShoping từ DTO
        var cartItemShoping = new CartItemShoping
        {
            CartId = _cartItemShopingDto.CartId,
            ProductId = _cartItemShopingDto.ProductId
        };

        _context.CartItemShoping.Add(cartItemShoping);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCartsByCartItemID), new { ItemID = _cartItemShopingDto.CartItemId }, cartItemShoping);
    }
    
    
    [HttpGet("CartID/{UserId}")]
    public async Task<ActionResult<CartShoping>> GetCartsById(int UserId)
    {
        var carts = await _context.CartShoping.FirstOrDefaultAsync(c => c.UserId == UserId);
    
        if (carts == null)
        {
            return NotFound();
        }
    
        return carts;
    }
    [HttpGet("ItemShoping/{ItemID}")]
    public async Task<ActionResult<CartItemShoping>> GetCartsByCartItemID(int ItemID)
    {
        var cartItemShoping = await _context.CartItemShoping
            .Include(c => c.CartShoping)  // Include CartShoping navigation property
            .Include(c => c.Product)      // Include Product navigation property
            .FirstOrDefaultAsync(c => c.CartItemId == ItemID);  // Use FirstOrDefaultAsync instead of FindAsync

    
        if (cartItemShoping == null)
        {
            return NotFound();
        }
    
        return cartItemShoping;
    }
    

    [HttpGet("Get_Item_Shoping/{CartId}")]
    public async Task<ActionResult<CartItemShoping>> GetItemShoping(int CartId)
    {
        var getitemshoping = await _context.CartItemShoping
            .Include(c => c.CartShoping)
            .Include(c => c.Product)
            .Where(c => c.CartId == CartId)
            .ToListAsync();
        if (getitemshoping == null || !getitemshoping.Any())
        {
            return NotFound();
        }

        return Ok(getitemshoping);
    }

    [HttpDelete("Delete_Cart/{cartId}")]
    public async Task<ActionResult<CartItemShoping>> DeleteCart(int cartId)
    {
        var findresult =
            await (from c in _context.CartItemShoping where c.CartItemId == cartId select c).FirstOrDefaultAsync();
        if (findresult == null)
        {
            return NotFound("khong co hang hoa");
        }
        _context.CartItemShoping.Remove(findresult);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "User deleted successfully" });
    }
}