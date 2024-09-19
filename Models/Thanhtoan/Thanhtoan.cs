namespace Web_2.Models.Thanhtoan;

public class ThanhToan
{
    public int Id { get; set; }
    public int Idnguoimua { get; set; }
    public int Idnguoiban { get; set; }
    public int ProductId { get; set; }
    public int Soluong { get; set; }
    public int Dongia { get; set; }
    public int Tongtien { get; set; }
    public DateTime Ngaythanhtoan { get; set; }
    public int Phuongthucthanhtoan { get; set; }
}