using Asp.Versioning;
using Dtos;
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

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        /// <summary>
        /// Returns all products.
        /// </summary>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllProducts(cancellationToken);
            return Ok(products);
        }

        /// <summary>
        /// Returns all products with pagination.
        /// </summary>
        [HttpGet]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(PagedResult<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginationQuery q, CancellationToken ct)
        {
            var page = q.PageNumber <= 0 ? 1 : q.PageNumber;
            var size = q.PageSize <= 0 ? 10 : q.PageSize;

            var result = await _productService.GetProductsPaged(page, size, ct);
            return Ok(result);
        }

        /// <summary>
        /// Returns a single product by ID.
        /// </summary>
        [HttpGet("{productId:int}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int productId, CancellationToken cancellationToken)
        {
            var product = await _productService.GetById(productId, cancellationToken);

            if (product is null)
            {
                return NotFound($"Product with ID {productId} was not found.");
            }

            return Ok(product);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
        {
            var createdProduct = await _productService.Create(request, cancellationToken);

            return CreatedAtAction(nameof(Get), new { productId = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Updates the stock quantity of a product.
        /// </summary>
        [HttpPatch("stock")]
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
    }
}
