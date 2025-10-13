using AutoMapper;
using PhisioDeviceAPI.DTOs.Products;
using PhisioDeviceAPI.Models;
using PhisioDeviceAPI.Repository;

namespace PhisioDeviceAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProductListItemDto>> GetAllAsync(string? category, string? brand, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(category, brand, cancellationToken);
            return _mapper.Map<IReadOnlyList<ProductListItemDto>>(products);
        }

        public async Task<ProductDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);
            return product == null ? null : _mapper.Map<ProductDetailDto>(product);
        }

        public async Task<ProductDetailDto> CreateAsync(CreateProductDto dto, IEnumerable<string>? imageUrls, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(dto);
            if (imageUrls != null)
            {
                var urls = imageUrls.Where(u => !string.IsNullOrWhiteSpace(u)).ToList();
                if (urls.Count > 0)
                {
                    product.ImageUrl = urls[0];
                    product.Images = urls.Select(u => new ProductImage { Url = u }).ToList();
                }
            }
            var created = await _productRepository.AddAsync(product, cancellationToken);
            return _mapper.Map<ProductDetailDto>(created);
        }

        public async Task<ProductDetailDto> UpdateAsync(int id, UpdateProductDto dto, IEnumerable<string>? imageUrls, CancellationToken cancellationToken)
        {
            var existing = await _productRepository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Product not found");
            _mapper.Map(dto, existing);
            if (imageUrls != null)
            {
                var urls = imageUrls.Where(u => !string.IsNullOrWhiteSpace(u)).ToList();
                if (urls.Count > 0)
                {
                    existing.ImageUrl = urls[0];
            // If client explicitly set ImageUrl in dto, honor it as main image
            if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
            {
                existing.ImageUrl = dto.ImageUrl;
                if (existing.Images.All(i => i.Url != dto.ImageUrl))
                {
                    existing.Images.Add(new ProductImage { Url = dto.ImageUrl, ProductId = existing.Id });
                }
            }
                    foreach (var u in urls)
                    {
                        if (!existing.Images.Any(i => i.Url == u))
                        {
                            existing.Images.Add(new ProductImage { Url = u, ProductId = existing.Id });
                        }
                    }
                }
            }
            existing.UpdatedAtUtc = DateTime.UtcNow;
            var saved = await _productRepository.UpdateAsync(existing, cancellationToken);
            return _mapper.Map<ProductDetailDto>(saved);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await _productRepository.DeleteAsync(id, cancellationToken);
        }
    }
}


