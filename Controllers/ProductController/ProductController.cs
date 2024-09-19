using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_2.Data;
using Web_2.Migrations;
using Web_2.Minio;
using Web_2.Models.Product;

namespace Web_2.Controllers.ProductController;


[ApiController]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly MinIOService _minioService;
    
    public ProductController(AppDbContext context, MinIOService minioService)
    {
        _context = context;
        _minioService = minioService;
    }
    
    [HttpPut]
    [Route("AddProduct")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddProduct([FromForm] ProductCreateDto productDto)
    {
     
        // Kiểm tra nếu không có file ảnh được upload
        if (productDto.image == null || productDto.image.Length == 0)
        {
            return BadRequest("Không có file ảnh được upload.");
        }
    
        // Tạo tên file duy nhất cho ảnh
        var objectName = Guid.NewGuid() + Path.GetExtension(productDto.image.FileName);
    
        // Mở stream để upload ảnh lên MinIO
        using (var stream = productDto.image.OpenReadStream())
        {
            // Upload ảnh lên MinIO và lấy URL của ảnh
            objectName = await _minioService.UploadFileAsync(objectName, stream, productDto.image.ContentType);
        }
        
    
        // Tạo đối tượng Product từ ProductDto và gán URL ảnh vào thuộc tính Image
        var product = new Product
        {
            Name = productDto.name,
            Value = productDto.value,
            Image = objectName, // Gán URL của ảnh từ MinIO vào đây
            Decription = productDto.decription,
            Stockquantity = productDto.stockquantity,
            Sellerid = productDto.sellerid,
            Daycreated = DateTime.UtcNow
        };
    
        // Thêm Product vào cơ sở dữ liệu
        _context.product.Add(product);
        await _context.SaveChangesAsync();
        var imageUrl = await _minioService.GetFileUrl(objectName);
        product.Image = imageUrl;
    
        return Ok(product);
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

        var url = await _minioService.GetFileUrl(findResult.Image);
        findResult.Image = url;
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

        foreach (var product in products)
        {
            var imageurl = await _minioService.GetFileUrl(product.Image);
            product.Image = imageurl;
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

        if (getallprodut == null || getallprodut.Count == 0)
        {
            return NotFound("khong co san pham nao");
        }

        foreach (var product in getallprodut)
        {
            var imageurl = await _minioService.GetFileUrl(product.Image);
            product.Image = imageurl;
        }
        
        return Ok(getallprodut);
    }
    [HttpDelete]
    [Route("DeleteProduct")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var findProduct = await _context.product.FirstOrDefaultAsync(p => p.id == id);
        if (findProduct == null)
        {
            return NotFound("Khong co hang hoa");
        }
        // Tìm các CartItemShoping có ProductId tương ứng
        var cartItems = await _context.CartItemShoping
            .Where(ci => ci.ProductId == id)
            .ToListAsync();
        // Kiểm tra và xóa ảnh khỏi MinIO nếu có
        if (!string.IsNullOrEmpty(findProduct.Image))
        {
            var deleteresult = await _minioService.DeleteFileAsync(findProduct.Image);
            if (!deleteresult)
            {
                // Nếu có lỗi khi xóa ảnh, trả về thông báo lỗi
                return BadRequest(new { Message = "Không thể xóa ảnh từ MinIO" });
            }
        }
    // Xóa các CartItemShoping
        _context.CartItemShoping.RemoveRange(cartItems);
        // Xóa Product
        _context.product.Remove(findProduct);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "User deleted successfully" });
    }
    [HttpPatch]
    [Route("ChangeProduct/{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ChangeProduct(int id,[FromForm] ProductCreateDto productdto)
    {
        var findAsync = await _context.product.FirstOrDefaultAsync(p => p.id == id);
        if (findAsync == null)
        {
            return NotFound(new { Message = "Product not found" });
        }
        // Kiểm tra nếu có file ảnh mới được upload
        if (productdto.image != null && productdto.image.Length > 0)
        {
            // Xóa ảnh cũ khỏi MinIO nếu có
            if (!string.IsNullOrEmpty(findAsync.Image))
            {
                var deleteResult = await _minioService.DeleteFileAsync(findAsync.Image);
                if (!deleteResult)
                {
                    return BadRequest("Không thể xóa ảnh cũ trên MinIO.");
                }
            }

            // Tạo tên file mới cho ảnh và upload ảnh lên MinIO
            var newImageName = Guid.NewGuid() + Path.GetExtension(productdto.image.FileName);

            using (var stream = productdto.image.OpenReadStream())
            {
                newImageName = await _minioService.UploadFileAsync(newImageName, stream, productdto.image.ContentType);
            }

            // Lấy URL của ảnh từ MinIO và cập nhật thuộc tính Image
            // var imageUrl = await _minioService.GetFileUrl(newImageName);
            // findAsync.Image = imageUrl; // Cập nhật URL mới của ảnh
            findAsync.Image = newImageName;
        }
        
        // update Product
        findAsync.Name = productdto.name;
        findAsync.Value = productdto.value;
        findAsync.Decription = productdto.decription;
        findAsync.Stockquantity = productdto.stockquantity;
        findAsync.Sellerid = productdto.sellerid;
        findAsync.Daycreated = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return Ok(new { Message = "User updated successfully" });
    }
}
