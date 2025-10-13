namespace PhisioDeviceAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; set; }
    }
}


