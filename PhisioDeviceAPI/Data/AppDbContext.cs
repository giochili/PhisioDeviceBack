using Microsoft.EntityFrameworkCore;
using PhisioDeviceAPI.Models;

namespace PhisioDeviceAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(200);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Role).IsRequired().HasMaxLength(32);
                entity.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Sku).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Brand).HasMaxLength(150);
                entity.Property(p => p.Category).HasMaxLength(150);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
                entity.HasIndex(p => p.Sku).IsUnique();
                entity.HasMany(p => p.Images)
                      .WithOne(i => i.Product)
                      .HasForeignKey(i => i.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("ProductImages");
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Url).IsRequired();
            });
        }
    }
}
