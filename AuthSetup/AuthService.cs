using Web_2.Data;
using Web_2.Models;

namespace Web_2.AuthSetup;

public class AuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public User AuthenticateUser(string account, string password)
    {
        // Tìm user trong cơ sở dữ liệu
        var user = _context.USER.FirstOrDefault(u => u.account == account);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.password))
        {
            return null; // Người dùng không tồn tại hoặc mật khẩu sai
        }

        return user; // Trả về user nếu thông tin đăng nhập đúng
    }
}