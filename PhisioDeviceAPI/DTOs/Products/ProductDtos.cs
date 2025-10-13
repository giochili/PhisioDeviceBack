namespace PhisioDeviceAPI.DTOs.Products
{
    public class ProductListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }

    public class ProductDetailDto : ProductListItemDto
    {
        public string? Description { get; set; }
        public string Sku { get; set; } = string.Empty;
        public int Stock { get; set; }
    }

    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
    }

    public class UpdateProductDto : CreateProductDto
    {
        public string? ImageUrl { get; set; }
    }
}


