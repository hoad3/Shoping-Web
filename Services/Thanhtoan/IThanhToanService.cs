using Web_2.Models.Thanhtoan;

namespace Web_2.Services.Thanhtoan;

public interface IThanhToanService
{
    Task<ThanhToan> CreateThanhtoanAsync(ThanhtoanDto thanhtoanDto);
    int CalculateTongTien(int soluong, int dongia);
}