using AlzaTest.Messaging;
using Repositories.Abstraction;

namespace AlzaTest.Workers
{
    public sealed class StockUpdateProcessor : BackgroundService
    {
        private readonly ILogger<StockUpdateProcessor> _logger;
        private readonly IStockUpdateQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;

        public StockUpdateProcessor(
            ILogger<StockUpdateProcessor> logger,
            IStockUpdateQueue queue,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _queue = queue;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StockUpdateProcessor started.");
            try
            {
                await foreach (var msg in _queue.ReadAllAsync(cancellationToken))
                {
                    try
                    {
                        // Create a fresh scope for each message (gets a fresh DbContext/Repo)
                        using var scope = _scopeFactory.CreateScope();
                        var repo = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                        var ok = await repo.UpdateStockQuantity(msg.ProductId, msg.NewQuantity, cancellationToken);
                        if (!ok)
                            _logger.LogWarning("Product {ProductId} not found (corrId {Corr}).", msg.ProductId, msg.CorrelationId);
                        else
                            _logger.LogInformation("Stock set to {Qty} for {ProductId} (corrId {Corr}).",
                                msg.NewQuantity, msg.ProductId, msg.CorrelationId);
                    }
                    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                    {
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed processing stock update for {ProductId} (corrId {Corr}).",
                            msg.ProductId, msg.CorrelationId);
                    }
                }
            }
            catch (OperationCanceledException) {}
            finally
            {
                _logger.LogInformation("StockUpdateProcessor stopping.");
            }
        }
    }
}
