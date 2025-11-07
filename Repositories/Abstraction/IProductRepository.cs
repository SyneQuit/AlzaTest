using Data.Entities;
using Dtos;

namespace Repositories.Abstraction
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetAll(CancellationToken cancellationToken);
        Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Product> Create(Product product, CancellationToken cancellationToken);
        Task<bool> UpdateStockQuantity(int productId, int stockQuantity, CancellationToken cancellationToken);
        Task<bool> ExistsByName(string name, CancellationToken cancellationToken);
        Task<int> Count(CancellationToken cancellationToken);
        Task<IReadOnlyList<ProductDto>> GetPage(int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
