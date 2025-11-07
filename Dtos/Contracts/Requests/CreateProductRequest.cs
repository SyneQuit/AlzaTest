using System.ComponentModel.DataAnnotations;

namespace Dtos.Contracts.Requests
{
    public class CreateProductRequest
    {
        [Required, StringLength(200)]
        public required string Name { get; set; }

        [Required, Url, StringLength(2048)]
        public required string Url { get; set; }

        [Range(0, 1_000_000)]
        public decimal Price { get; set; } = 0;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; } = 0;
    }
}
