using Dtos.Contracts.Messaging;

namespace AlzaTest.Messaging
{
    public interface IStockUpdateQueue
    {
        ValueTask EnqueueAsync(StockUpdateMessage message, CancellationToken ct);
        IAsyncEnumerable<StockUpdateMessage> ReadAllAsync(CancellationToken ct);
    }
}
