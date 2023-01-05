using Application.Products;
using Backend_Challenge_20220626.Controllers;
using Domain.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _service;
        private readonly ProductsController _controller;
        private readonly IFixture _fixture;
        public ProductControllerTests()
        {
            _fixture = new Fixture();
            _service = new Mock<IProductService>();
            _controller = new ProductsController(_service.Object);
        }

        [Fact]
        public void ShouldCreate()
        {
            _controller.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Return_Product_Referencing_Code()
        {
            // Arrange
            string code = _fixture.Create<string>();
            var mockResponse = _fixture.Create<Product>();

            // Setup
            _service.Setup(x => x.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetByCode(code) as OkObjectResult;
            var response = result.Value;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(mockResponse);
        }

        [Fact]
        public async Task Should_Return_NotFound_When_Product_Not_Found()
        {
            // Arrange
            string code = _fixture.Create<string>();

            // Setup
            _service.Setup(x => x.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetByCode(code) as NotFoundObjectResult;
            var response = result.Value;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            response.Should().NotBeNull();
        }


        [Fact]
        public async Task Sould_Return_List_Products()
        {
            // Arrange
            var mockResponse = _fixture.Create<List<Product>>();

            // Setup
            _service.Setup(x => x.GetPaged(It.IsAny<int>(),It.IsAny<int>())).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAll(0,10) as OkObjectResult;
            var response = result.Value;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(mockResponse);
        }


        [Fact]
        public async Task Sould_BadRequest_When_PageIs_Negative()
        {
            // Arrange
            var mockResponse = _fixture.Create<List<Product>>();

            // Setup
            _service.Setup(x => x.GetPaged(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAll(-1, 10) as BadRequestObjectResult;
            var response = result.Value;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            response.Should().NotBeNull();
        }
    }
}
