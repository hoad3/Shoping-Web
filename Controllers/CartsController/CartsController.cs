using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_2.Data;
using Web_2.Minio;
using Web_2.Models;
using Web_2.Models.Carts;

namespace Web_2.Controllers;


[ApiController]
public class CartsController: ControllerBase
{
    private readonly AppDbContext _context;
    private readonly MinIOService _minioService;
    public CartsController(AppDbContext context, MinIOService minioService)
    {
        _context = context;
        _minioService = minioService;
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
    [HttpPost("AddCartItem/{CartId}/{ProductId}")]
    public async Task<ActionResult<CartItemShoping>> AddCartItem(int CartId, int ProductId)
    {
        // Tìm giỏ hàng bằng CartId từ URL
        var cart = await _context.CartShoping.FindAsync(CartId);
        if (cart == null)
        {
            return NotFound(new { Message = "Cart not found" });
        }

        // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
        var existingCartItem = await _context.CartItemShoping
            .FirstOrDefaultAsync(ci => ci.CartId == CartId && ci.ProductId == ProductId);
        
        // // Tạo đối tượng CartItemShoping
        // var cartItemShoping = new CartItemShoping
        // {
        //     CartId = CartId,
        //     ProductId = ProductId
        // };
        //
        // _context.CartItemShoping.Add(cartItemShoping);
        if (existingCartItem != null)
        {
            // Nếu sản phẩm đã tồn tại, tăng số lượng
            existingCartItem.Quantity += 1;
        }
        // Nếu sản phẩm chưa tồn tại, tạo mới CartItemShoping với số lượng được chỉ định
        var newCartItem  = new CartItemShoping
        {
            CartId = CartId,
            ProductId = ProductId,
            Quantity = 1 // Set the initial quantity
        };
        _context.CartItemShoping.Add(newCartItem );
        await _context.SaveChangesAsync();

        // Trả về kết quả đã được tạo hoặc cập nhật
        return CreatedAtAction(nameof(GetCartsByCartItemID), new { ItemID = newCartItem.CartItemId }, newCartItem);
    }
    
    [HttpPatch("UpdateCartItemQuantity/{CartId}/{ProductId}")]
    public async Task<ActionResult<CartItemShoping>> UpdateCartItemQuantity(int CartId, int ProductId, [FromBody] CartItemShopingDto dto)
    {
        // Tìm sản phẩm trong giỏ hàng bằng cartItemId
        var cartItem = await _context.CartItemShoping
            .FirstOrDefaultAsync(ci => ci.CartId == CartId && ci.ProductId == ProductId);
        if (cartItem == null)
        {
            return NotFound(new { Message = "Cart item not found" });
        }

        // Cập nhật số lượng
        cartItem.Quantity += dto.quantity;

        // Nếu quantity trở về 0 hoặc âm, có thể xóa sản phẩm
        if (cartItem.Quantity <= 0)
        {
            _context.CartItemShoping.Remove(cartItem);
        }
        else
        {
            _context.CartItemShoping.Update(cartItem);
        }

        await _context.SaveChangesAsync();

        // Trả về kết quả đã cập nhật
        return Ok(cartItem);
    }
    [HttpGet("CheckCartItem/{cartId}/{productId}")]
    public async Task<ActionResult<bool>> CheckCartItem(int cartId, int productId)
    {
        var cartItem = await _context.CartItemShoping
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        return Ok(cartItem != null);
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

        foreach (var item in getitemshoping)
        {
            if (!string.IsNullOrEmpty(item.Product.Image))
            {
                item.Product.Image = await _minioService.GetFileUrl(item.Product.Image);
            }
        }

        return Ok(getitemshoping);
    }

    [HttpDelete("Delete_Cart/{cartIteamId}")]
    public async Task<ActionResult<CartItemShoping>> DeleteCart(int cartIteamId)
    {
        var findresult =
            await (from c in _context.CartItemShoping where c.CartItemId == cartIteamId select c).FirstOrDefaultAsync();
        if (findresult == null)
        {
            return NotFound("khong co hang hoa");
        }
        _context.CartItemShoping.Remove(findresult);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "User deleted successfully" });
    }

    // [HttpDelete("Delete_CartItem/{CartItemId}")]
    //
    // public async Task<ActionResult<CartItemShoping>> DeleteCartItem(int CartItemId)
    // {
    //     var findCartItem = 
    //         await (from c in _context.CartItemShoping where c.CartItemId == )
    // }
}