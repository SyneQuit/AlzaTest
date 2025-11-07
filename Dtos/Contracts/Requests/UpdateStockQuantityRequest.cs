using System.ComponentModel.DataAnnotations;

namespace Dtos.Contracts.Requests
{
    public class UpdateStockQuantityRequest
    {
        [Range(1, int.MaxValue)]
        public required int Id { get; set; }

        [Range(0, int.MaxValue)]
        public required int StockQuantity { get; set; }
    }
}
