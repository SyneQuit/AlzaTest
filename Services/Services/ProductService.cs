using Data.Entities;
using Dtos;
using Dtos.Contracts.Requests;
using Repositories.Abstraction;
using Services.Abstraction;

namespace Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts(CancellationToken cancellationToken)
        {
            return await _productRepository.GetAll(cancellationToken);
        }

        public async Task<ProductDto?> GetById(int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                throw new ArgumentException("Id must be greater than zero.", nameof(id));
            }
                
            return await _productRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<ProductDto> Create(CreateProductRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Url))
                throw new ArgumentException("Name and url of product cannot be empty.", nameof(request));

            if (await _productRepository.ExistsByName(request.Name, cancellationToken))
            {
                throw new InvalidOperationException($"Product with name '{request.Name}' already exists.");
            }

            var product = MapToProductEntity(request);

            var saved =  await _productRepository.Create(product, cancellationToken);

            return MapToProductDto(saved);
        }

        public async Task<bool> UpdateStockQuantity(UpdateStockQuantityRequest request, CancellationToken cancellationToken)
        {
            return await _productRepository.UpdateStockQuantity(request.Id, request.StockQuantity, cancellationToken);
        }

        public async Task<PagedResult<ProductDto>> GetProductsPaged(int page, int size, CancellationToken ct)
        {
            if (page < 1) throw new ArgumentException("PageNumber must be >= 1", nameof(page));
            if (size < 1) throw new ArgumentException("PageSize must be >= 1", nameof(size));

            var total = await _productRepository.Count(ct);
            var items = await _productRepository.GetPage(page, size, ct);

            return new PagedResult<ProductDto>
            {
                Items = items,
                PageNumber = page,
                PageSize = size,
                TotalCount = total
            };
        }

        private static Product MapToProductEntity(CreateProductRequest request) => new Product
        {
            Name = request.Name,
            Url = request.Url,
            Price = request.Price,
            Description = request.Description,
            StockQuantity = request.StockQuantity,
        };

        private static ProductDto MapToProductDto(Product p) => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            Url = p.Url
        };
    }
}
