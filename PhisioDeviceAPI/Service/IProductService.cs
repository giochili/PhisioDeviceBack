using PhisioDeviceAPI.DTOs.Products;

namespace PhisioDeviceAPI.Service
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductListItemDto>> GetAllAsync(string? category, string? brand, CancellationToken cancellationToken);
        Task<ProductDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<ProductDetailDto> CreateAsync(CreateProductDto dto, IEnumerable<string>? imageUrls, CancellationToken cancellationToken);
        Task<ProductDetailDto> UpdateAsync(int id, UpdateProductDto dto, IEnumerable<string>? imageUrls, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}


