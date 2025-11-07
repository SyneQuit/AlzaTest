using AlzaTest.Controllers;
using Dtos;
using Dtos.Contracts.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.Api.Tests.TestData;
using Repositories.Abstraction;
using Services.Abstraction;
using Services.Services;

namespace Products.Api.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _service = new(MockBehavior.Strict);

        private ProductController Sut() => new(_service.Object);

        [Fact]
        public async Task GetAll_returns_200_with_products()
        {
            // Arrange
            var expected = ProductFakes.Dtos(2).ToList();
            _service.Setup(s => s.GetAllProducts(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expected);

            var controller = Sut();

            // Act
            var result = await controller.GetAll(CancellationToken.None);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.StatusCode.Should().Be(StatusCodes.Status200OK);
            ok.Value.Should().BeEquivalentTo(expected);

            _service.VerifyAll();
        }

        [Fact]
        public async Task Get_returns_200_with_product_when_found()
        {
            // Arrange
            var dto = ProductFakes.Dto(42);
            _service.Setup(s => s.GetById(42, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(dto);

            var controller = Sut();

            // Act
            var result = await controller.Get(42, CancellationToken.None);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.StatusCode.Should().Be(StatusCodes.Status200OK);
            ok.Value.Should().BeEquivalentTo(dto);

            _service.VerifyAll();
        }

        [Fact]
        public async Task Get_returns_404_when_not_found()
        {
            _service.Setup(s => s.GetById(99, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((ProductDto?)null);

            var controller = Sut();

            var result = await controller.Get(99, CancellationToken.None);

            result.Should().BeOfType<NotFoundObjectResult>();
            _service.VerifyAll();
        }

        [Fact]
        public async Task Create_returns_201_and_location_with_created_product()
        {
            // Arrange
            var request = ProductFakes.CreateRequestValid();
            var created = ProductFakes.Dto(7);

            _service.Setup(s => s.Create(It.IsAny<CreateProductRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(created);

            var controller = Sut();

            // Act
            var result = await controller.Create(request, CancellationToken.None);

            // Assert
            var createdRes = result as CreatedAtActionResult;
            createdRes.Should().NotBeNull();
            createdRes!.StatusCode.Should().Be(StatusCodes.Status201Created);
            createdRes.ActionName.Should().Be(nameof(ProductController.Get));
            createdRes.RouteValues!["productId"].Should().Be(created.Id);
            createdRes.Value.Should().BeEquivalentTo(created);

            _service.VerifyAll();
        }

        [Fact]
        public async Task Create_throws_when_name_exists_and_does_not_call_Create()
        {
            // Arrange
            var req = new CreateProductRequest
            {
                Name = "Wireless Mouse",
                Url = "https://example.com/mouse",
                Price = 10m,
                Description = "x",
                StockQuantity = 5
            };

            var repo = new Mock<IProductRepository>(MockBehavior.Strict);

            repo.Setup(r => r.ExistsByName("Wireless Mouse", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            repo.Setup(r => r.Create(It.IsAny<Data.Entities.Product>(), It.IsAny<CancellationToken>()))
                .Throws(new Xunit.Sdk.XunitException("Create should not be called when name exists"));

            var sut = new ProductService(repo.Object);

            // Act
            var act = () => sut.Create(req, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                     .WithMessage($"Product with name 'Wireless Mouse' already exists.");

            repo.Verify(r => r.ExistsByName("Wireless Mouse", It.IsAny<CancellationToken>()), Times.Once);
            repo.Verify(r => r.Create(It.IsAny<Data.Entities.Product>(), It.IsAny<CancellationToken>()), Times.Never);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateStockQuantity_returns_204_when_updated()
        {
            var request = ProductFakes.UpdateStockRequestValid(1, 50);

            _service.Setup(s => s.UpdateStockQuantity(request, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

            var controller = Sut();

            var result = await controller.UpdateStockQuantity(request, CancellationToken.None);

            result.Should().BeOfType<NoContentResult>();
            _service.VerifyAll();
        }

        [Fact]
        public async Task UpdateStockQuantity_returns_404_when_product_missing()
        {
            var request = ProductFakes.UpdateStockRequestValid(999, 50);

            _service.Setup(s => s.UpdateStockQuantity(request, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(false);

            var controller = Sut();

            var result = await controller.UpdateStockQuantity(request, CancellationToken.None);

            result.Should().BeOfType<NotFoundObjectResult>();
            _service.VerifyAll();
        }
    }
}
