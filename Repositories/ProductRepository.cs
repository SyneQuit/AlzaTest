using Data.Entities;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstraction;
using System.Linq.Expressions;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ProductDto>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Products
                .AsNoTracking()
                .Select(MapToProductDto)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductDto?> GetByIdAsync(int productId, CancellationToken cancellationToken)
        {
            return await _context.Products
            .Where(p => p.Id == productId)
            .Select(MapToProductDto)
            .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Product> Create(Product product, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return product;
        }

        public Task<bool> ExistsByName(string name, CancellationToken ct)
        {
            return _context.Products
                .AsNoTracking()
                .AnyAsync(p => p.Name == name, ct);
        }

        public async Task<bool> UpdateStockQuantity(int productId, int stockQuantity, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

            if (product is null)
            {
                return false;
            }

            product.StockQuantity = stockQuantity;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<int> Count(CancellationToken ct)
        {
            return _context.Products
                .AsNoTracking()
                .CountAsync(ct);
        }


        public async Task<IReadOnlyList<ProductDto>> GetPage(int page, int size, CancellationToken ct)
        {
            var skip = (page - 1) * size;
            return await _context.Products
                .OrderBy(p => p.Id)
                .Skip(skip)
                .Take(size)
                .Select(MapToProductDto)
                .ToListAsync(ct);
        }

        private static Expression<Func<Product, ProductDto>> MapToProductDto => p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            Url = p.Url,
        };

    }
}
