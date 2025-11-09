using AlzaTest.Messaging;
using Asp.Versioning;
using Dtos;
using Dtos.Contracts.Messaging;
using Dtos.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;

namespace AlzaTest.Controllers
{
    /// <summary>
    /// Exposes product-related endpoints for querying, creating, and updating products.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStockUpdateQueue _stockQueue;

        public ProductController(IProductService productService, IStockUpdateQueue stockQueue)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _stockQueue = stockQueue ?? throw new ArgumentNullException(nameof(stockQueue));
        }

        /// <summary>v1: Returns all products without pagination.</summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>200 OK with a list of products.</returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllProducts(cancellationToken);
            return Ok(products);
        }

        /// <summary>v2: Returns products with pagination.</summary>
        /// <remarks>Defaults: pageNumber=1, pageSize=10.</remarks>
        /// <param name="q">Paging parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>200 OK with a paged result.</returns>
        [HttpGet]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(PagedResult<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginationQuery q, CancellationToken cancellationToken)
        {
            var page = q.PageNumber <= 0 ? 1 : q.PageNumber;
            var size = q.PageSize <= 0 ? 10 : q.PageSize;

            var result = await _productService.GetProductsPaged(page, size, cancellationToken);
            return Ok(result);
        }

        /// <summary>Returns a single product by ID.</summary>
        /// <param name="productId">Product identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>200 OK, or 404 if not found.</returns>
        [HttpGet("{productId:int}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int productId, CancellationToken cancellationToken)
        {
            if (productId < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            var product = await _productService.GetById(productId, cancellationToken);

            if (product is null)
            {
                return NotFound($"Product with ID {productId} was not found.");
            }

            return Ok(product);
        }

        /// <summary>Creates a new product.</summary>
        /// <param name="request">Product payload.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>201 Created with location header, or 409 if name is duplicate.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
        {
            var createdProduct = await _productService.Create(request, cancellationToken);

            return CreatedAtAction(nameof(Get), new { productId = createdProduct.Id }, createdProduct);
        }

        /// <summary>Updates the stock quantity of a product.</summary>
        /// <param name="request">Target product and the new stock quantity.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>204 when product is updated, or 404 when product wasnt found</returns>
        [HttpPatch("stock")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStockQuantity([FromBody] UpdateStockQuantityRequest request, CancellationToken cancellationToken)
        {
            var updated = await _productService.UpdateStockQuantity(request, cancellationToken);
            if (!updated)
                return NotFound($"Product with ID {request.Id} was not found.");

            return NoContent();
        }

        /// <summary>Queues an asynchronous stock update.</summary>
        /// <param name="request">Target product and the new stock quantity.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>202 Accepted with correlationId.</returns>
        [HttpPatch("stock")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStockQuantityAsync([FromBody] UpdateStockQuantityRequest request, CancellationToken cancellationToken)
        {
            var msg = new StockUpdateMessage
            {
                ProductId = request.Id,
                NewQuantity = request.StockQuantity
            };

            await _stockQueue.EnqueueAsync(msg, cancellationToken);

            return Accepted(new { message = "Stock update queued.", correlationId = msg.CorrelationId });
        }
    }
}
