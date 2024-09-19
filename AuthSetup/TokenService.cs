using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Web_2.Models;

namespace Web_2.AuthSetup;

public class TokenService
{
    private readonly string _secretKey;

    public TokenService(IConfiguration configuration)
    {
        _secretKey = configuration["JwtSettings:SecretKey"];
        
        // Kiểm tra nếu khóa bí mật không tồn tại
        if (string.IsNullOrEmpty(_secretKey))
        {
            throw new ArgumentNullException("Secret key không được cấu hình trong appsettings.json");
        }
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.id.ToString()),
                new Claim(ClaimTypes.Role, user.role.ToString()) // Thêm role vào token
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token); // Trả về token đã mã hóa
    }
}