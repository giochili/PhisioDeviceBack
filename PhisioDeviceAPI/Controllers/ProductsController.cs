using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhisioDeviceAPI.DTOs.Products;
using PhisioDeviceAPI.Service;

namespace PhisioDeviceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _env;

        public ProductsController(IProductService productService, IWebHostEnvironment env)
        {
            _productService = productService;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductListItemDto>>> GetAll([FromQuery] string? category, [FromQuery] string? brand, CancellationToken cancellationToken)
        {
            var items = await _productService.GetAllAsync(category, brand, cancellationToken);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDetailDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var item = await _productService.GetByIdAsync(id, cancellationToken);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [RequestSizeLimit(20_000_000)]
        public async Task<ActionResult<ProductDetailDto>> Create([FromForm] CreateProductDto dto, List<IFormFile>? images, CancellationToken cancellationToken)
        {
            var urls = new List<string>();
            if (images != null)
            {
                foreach (var img in images)
                {
                    var url = await SaveImageAsync(img, cancellationToken);
                    if (!string.IsNullOrWhiteSpace(url)) urls.Add(url);
                }
            }
            var created = await _productService.CreateAsync(dto, urls, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        [RequestSizeLimit(20_000_000)]
        public async Task<ActionResult<ProductDetailDto>> Update([FromRoute] int id, [FromForm] UpdateProductDto dto, List<IFormFile>? images, CancellationToken cancellationToken)
        {
            var urls = new List<string>();
            if (images != null)
            {
                foreach (var img in images)
                {
                    var url = await SaveImageAsync(img, cancellationToken);
                    if (!string.IsNullOrWhiteSpace(url)) urls.Add(url);
                }
            }
            var updated = await _productService.UpdateAsync(id, dto, urls, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            await _productService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        private async Task<string?> SaveImageAsync(IFormFile? image, CancellationToken cancellationToken)
        {
            if (image == null || image.Length == 0) return null;
            var webRoot = _env.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
            {
                webRoot = Path.Combine(_env.ContentRootPath, "wwwroot");
            }
            var uploads = Path.Combine(webRoot, "images");
            Directory.CreateDirectory(uploads);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(uploads, fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await image.CopyToAsync(stream, cancellationToken);
            }
            var relativeUrl = $"/images/{fileName}";
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return new Uri(new Uri(baseUrl), relativeUrl).ToString();
        }
    }
}


