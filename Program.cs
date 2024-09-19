using System.Text;
using Web_2.Controllers;
using Web_2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Minio.AspNetCore;
using Nancy.Authentication.JwtBearer;
using Web_2.AuthSetup;
using Web_2.Data;
using Web_2.Minio;
using Web_2.Services.Thanhtoan;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
//MinIO
        builder.Services.AddMinio(options =>
        {
            options.Endpoint = "localhost:9000";
            options.AccessKey = "y8tccwXv833XbwUf5vTr";
            options.SecretKey = "1DhEUwgqVhTzgOlYYqMosRhnmmptz4l2UkaR07JG";
        });
        builder.Services.AddScoped<MinIOService>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<IThanhToanService, ThanhToanService>();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

// Add services to the container.
        builder.Services.AddControllers();
        
        // Configure database connection
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });            

        
        
        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(options =>
        {
            options.AllowAnyHeader();
            options.AllowAnyMethod();
            options.WithOrigins("*");
        });

        
        app.MapControllers();
        app.Run();
        
    }
    
}
