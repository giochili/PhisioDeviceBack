using Microsoft.EntityFrameworkCore;
using PhisioDeviceAPI.Data;
using PhisioDeviceAPI.Models;

namespace PhisioDeviceAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync(string? category, string? brand, CancellationToken cancellationToken)
        {
            var query = _dbContext.Products.Include(p => p.Images).AsQueryable();
            if (!string.IsNullOrWhiteSpace(category)) query = query.Where(p => p.Category == category);
            if (!string.IsNullOrWhiteSpace(brand)) query = query.Where(p => p.Brand == brand);
            return await query.OrderByDescending(p => p.CreatedAtUtc).ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken)
        {
            await _dbContext.Products.AddAsync(product, cancellationToken);
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
            return product;
        }

        public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (entity != null)
            {
                _dbContext.Products.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}


