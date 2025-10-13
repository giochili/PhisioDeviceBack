using PhisioDeviceAPI.Models;

namespace PhisioDeviceAPI.Repository
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetAllAsync(string? category, string? brand, CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Product> AddAsync(Product product, CancellationToken cancellationToken);
        Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}


