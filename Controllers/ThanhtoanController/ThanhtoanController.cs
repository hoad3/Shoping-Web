using Microsoft.AspNetCore.Mvc;
using Web_2.Data;
using Web_2.Models.Thanhtoan;
using Web_2.Services.Thanhtoan;

namespace Web_2.Controllers;

public class ThanhtoanController: ControllerBase
{
    private readonly IThanhToanService _thanhtoanService;

    public ThanhtoanController(IThanhToanService thanhtoanService)
    {
        _thanhtoanService = thanhtoanService;
    }

    [HttpPost]
    [Route("Thanhtoan")]
    public async Task<IActionResult> CreateThanhtoan([FromBody] ThanhtoanDto thanhtoanDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Sử dụng service để tạo thanh toán
        var thanhtoan = await _thanhtoanService.CreateThanhtoanAsync(thanhtoanDto);

        return Ok(new { message = "Thanh toán đã được tạo thành công", thanhtoan });
    }
}