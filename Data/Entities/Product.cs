namespace Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }
        public decimal Price { get; set; } = 0;
        public string? Description { get; set; }
        public int StockQuantity { get; set; } = 0;
    }
}
