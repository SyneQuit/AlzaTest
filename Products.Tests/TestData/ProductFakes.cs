using Dtos;
using Dtos.Contracts.Requests;

namespace Products.Api.Tests.TestData
{
    internal static class ProductFakes
    {
        public static ProductDto Dto(int id = 1) => new()
        {
            Id = id,
            Name = $"Product {id}",
            Url = $"https://example.com/p/{id}",
            Description = "Sample description",
            Price = 19.99m,
            StockQuantity = 10
        };

        public static IEnumerable<ProductDto> Dtos(int count = 3)
        {
            for (int i = 1; i <= count; i++)
                yield return Dto(i);
        }

        public static CreateProductRequest CreateRequestValid() => new()
        {
            // Use the property names you currently have in your project.
            // If you followed the latest advice, these are Name/Url/Price.
            Name = "New Product",
            Url = "https://example.com/new",
            Description = "Brand new",
            Price = 29.99m,
            StockQuantity = 5
        };

        public static UpdateStockQuantityRequest UpdateStockRequestValid(int productId = 1, int stock = 25) => new()
        {
            Id = productId,
            StockQuantity = stock
        };
    }
}