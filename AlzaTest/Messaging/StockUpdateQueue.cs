using Dtos.Contracts.Messaging;
using System.Threading.Channels;

namespace AlzaTest.Messaging
{
    public sealed class StockUpdateQueue : IStockUpdateQueue
    {
        private readonly Channel<StockUpdateMessage> _channel;

        public StockUpdateQueue(int capacity = 1024)
        {
            var opts = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = false
            };
            _channel = Channel.CreateBounded<StockUpdateMessage>(opts);
        }

        public ValueTask EnqueueAsync(StockUpdateMessage message, CancellationToken ct)
            => _channel.Writer.WriteAsync(message, ct);

        public IAsyncEnumerable<StockUpdateMessage> ReadAllAsync(CancellationToken ct)
            => _channel.Reader.ReadAllAsync(ct);
    }
}
