using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Web_2.Models;
using Web_2.Models.Carts;
using Web_2.Models.Product;

namespace Web_2.Data;
public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> USER  { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<CartShoping> CartShoping { get; set; }
        public DbSet<CartItemShoping> CartItemShoping { get; set; }
        public DbSet<InformationUser> InformationUser { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Đảm bảo rằng EF biết các mối quan hệ và khóa chính
            
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
