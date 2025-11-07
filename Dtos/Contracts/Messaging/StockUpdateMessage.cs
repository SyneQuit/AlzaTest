namespace Dtos.Contracts.Messaging
{
    public sealed record StockUpdateMessage
    {
        public required int ProductId { get; init; }
        public required int NewQuantity { get; init; }
        public Guid CorrelationId { get; init; } = Guid.NewGuid();
        public DateTimeOffset InsertTime { get; init; } = DateTimeOffset.UtcNow;
    }
}
