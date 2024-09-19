using Web_2.Data;
using Web_2.Models.Thanhtoan;

namespace Web_2.Services.Thanhtoan;

public class ThanhToanService: IThanhToanService
{
    private readonly AppDbContext _context;

    public ThanhToanService(AppDbContext context)
    {
        _context = context;
    }

    // Hàm tính tổng tiền
    public int CalculateTongTien(int soluong, int dongia)
    {
        return soluong * dongia;
    }

    // Hàm tạo thanh toán
    public async Task<ThanhToan> CreateThanhtoanAsync(ThanhtoanDto thanhtoanDto)
    {
        // Tính tổng tiền
        int tongtien = CalculateTongTien(thanhtoanDto.Soluong, thanhtoanDto.Dongia);

        // Tạo đối tượng ThanhToan mới
        var thanhtoan = new ThanhToan
        {
            Idnguoimua = thanhtoanDto.Idnguoimua,
            Idnguoiban = thanhtoanDto.Idnguoiban,
            ProductId = thanhtoanDto.ProductId,
            Soluong = thanhtoanDto.Soluong,
            Dongia = thanhtoanDto.Dongia,
            Tongtien = tongtien,
            Ngaythanhtoan = thanhtoanDto.Ngaythanhtoan != default(DateTime) ? thanhtoanDto.Ngaythanhtoan : DateTime.UtcNow,
            Phuongthucthanhtoan = thanhtoanDto.Phuongthucthanhtoan
        };

        // Thêm thanh toán vào cơ sở dữ liệu
        _context.ThanhToan.Add(thanhtoan);
        await _context.SaveChangesAsync();

        return thanhtoan;
    }
}