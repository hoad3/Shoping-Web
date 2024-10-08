﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web_2.AuthSetup;
using Web_2.Data;
using Web_2.Models;

namespace Web_2.Controllers;


[ApiController]
// [Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AuthService _authService;
    private readonly TokenService _tokenService;

    public AuthController(AppDbContext context,TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UserRegisterDto user)
    {
        // Kiểm tra sự tồn tại của account
        var existingUser = await _context.USER.FirstOrDefaultAsync(u => u.account == user.account);
        if (existingUser != null)
        {
            return BadRequest("Account already exists.");
        }
        
        var users = new User()
        {
            id =user.id,
            account = user.account,
            password = user.password,
        };
        await _context.USER.AddAsync(users);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserRegisterDto user)
    {
        
        // Check if user exists
        // var dbUser = _context.USER.FirstOrDefault(u => u.account == user.account && u.password == user.password);
        // if (dbUser == null)
        // {
        //     return Unauthorized("Invalid credentials");
        // }
        //
        // // Generate JWT Token
        // var tokenHandler = new JwtSecurityTokenHandler();
        // var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"u8.ToArray();
        // var tokenDescriptor = new SecurityTokenDescriptor
        // {
        //     Subject = new ClaimsIdentity(new Claim[]
        //     {
        //         new Claim(ClaimTypes.Name, dbUser.id.ToString())
        //     }),
        //     Expires = DateTime.UtcNow.AddHours(1),
        //     SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key[..128]), SecurityAlgorithms.HmacSha256Signature)
        // };
        // var token = tokenHandler.CreateToken(tokenDescriptor);
        // var tokenString = tokenHandler.WriteToken(token);
        //
        // return Ok(new
        // {
        //     Token = tokenString,
        //     Userid = dbUser.id
        //     
        // });
        // Kiểm tra thông tin đăng nhập
        var dbUser = _context.USER.FirstOrDefault(u => u.account == user.account && u.password == user.password);
        if (dbUser == null)
        {
            Console.WriteLine($"No user found with account: {user.account} and password: {user.password}");
            return Unauthorized("Tài khoản hoặc mật khẩu không chính xác");
        }

        // Tạo JWT token
        var token = _tokenService.GenerateToken(dbUser);

        return Ok(new
        {
            Token = token,
            Userid = dbUser.id,
            Role = dbUser.role // Trả về role của người dùng
        });
    }
    [HttpPost]
    [Route("change-password")]
    public async Task<IActionResult> ChangePassword(UserChangePasswordDto user)
    {
        // Kiểm tra sự tồn tại của account
        var existingUser = await _context.USER.FirstOrDefaultAsync(u => u.account == user.account);
        if (existingUser == null)
        {
            return NotFound("Account does not exist.");
        }

        // Cập nhật mật khẩu mới
        existingUser.password = user.newPassword;
        _context.USER.Update(existingUser);
        await _context.SaveChangesAsync();

        return Ok("Password changed successfully");
    }

    [HttpGet]
    [Route("Find_Account/{userId}")]
    public async Task<IActionResult> FindAccount(int userId)
    {
        var dbUser = await _context.USER.Where(u => u.id == userId).ToListAsync();
        if (dbUser == null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(dbUser);
    }
}
