using Data.Entities;
using Dtos;
using Dtos.Contracts.Requests;

namespace Services.Abstraction
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts(CancellationToken cancellationToken);
        Task<ProductDto?> GetById(int id, CancellationToken cancellationToken);
        Task<ProductDto> Create(CreateProductRequest request, CancellationToken cancellationToken);
        Task<bool> UpdateStockQuantity(UpdateStockQuantityRequest request, CancellationToken cancellationToken);
        Task<PagedResult<ProductDto>> GetProductsPaged(int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
