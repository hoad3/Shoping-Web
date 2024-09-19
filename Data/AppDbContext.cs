using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Web_2.Models;
using Web_2.Models.Carts;
using Web_2.Models.Product;
using Web_2.Models.Thanhtoan;

namespace Web_2.Data;
public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> USER  { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<CartShoping> CartShoping { get; set; }
        public DbSet<CartItemShoping> CartItemShoping { get; set; }
        public DbSet<InformationUser> InformationUser { get; set; }
        public DbSet<ThanhToan> ThanhToan { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Đảm bảo rằng EF biết các mối quan hệ và khóa chính
            // Thiết lập khóa chính cho bảng Thanhtoan
            modelBuilder.Entity<ThanhToan>()
                .ToTable("ThanhToan") // Xác định tên bảng là "ThanhToan"
                .HasKey(t => t.Id);
            
            modelBuilder.Entity<ThanhToan>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd(); // Cấu hình tự động tăng giá trị cho cột Id
            
            // Thiết lập khóa ngoại Idnguoimua liên kết với bảng User
            modelBuilder.Entity<ThanhToan>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.Idnguoimua)
                .OnDelete(DeleteBehavior.Restrict);  // Tùy chọn hành vi xóa

            // Thiết lập khóa ngoại Idnguoiban liên kết với bảng User
            modelBuilder.Entity<ThanhToan>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.Idnguoiban)
                .OnDelete(DeleteBehavior.Restrict);

            // Thiết lập khóa ngoại ProductId liên kết với bảng Product
            modelBuilder.Entity<ThanhToan>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Thiết lập khóa chính cho bảng CartShoping
            modelBuilder.Entity<CartShoping>()
                .HasKey(c => c.CartId);
            // Cấu hình thuộc tính CreatedAt
            
            modelBuilder.Entity<CartShoping>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            // Cấu hình CartId là auto-increment
            
            modelBuilder.Entity<CartShoping>()
                .Property(c => c.CartId)
                .ValueGeneratedOnAdd();
            
            // Cấu hình khóa chính cho thực thể CartItemShoping
            modelBuilder.Entity<CartItemShoping>()
                .HasKey(c => c.CartItemId);
            // modelBuilder.Entity<CartItemShoping>().HasNoKey(); // Đánh dấu CartItemShoping là Keyless Entity Type
            
            modelBuilder.Entity<CartItemShoping>()
                .Property(c => c.CartItemId)
                .ValueGeneratedOnAdd();
            
            // Định nghĩa mối quan hệ một-nhiều giữa CartShoping và CartItemShoping
            modelBuilder.Entity<CartShoping>()
                .HasMany(c => c.CartItem)
                .WithOne(c => c.CartShoping)
                .HasForeignKey(c => c.CartId);
            
            //cấu hình khóa chính cho InformationUser
            modelBuilder.Entity<InformationUser>()
                .HasKey(i => i.Idname);
            
            // cấu hình mối quan hệ 1-1 cho InformationUser và User
            modelBuilder.Entity<User>()
                .HasOne<InformationUser>(u => u.InformationUser)
                .WithOne(i => i.User)
                .HasForeignKey<InformationUser>(i => i.User_id);
            
            modelBuilder.HasDefaultSchema("Data");
        }
    }
