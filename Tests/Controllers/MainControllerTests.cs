using Backend_Challenge_20220626.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Controllers
{
    public class MainControllerTests
    {
        private readonly MainController _controller;
        public MainControllerTests()
        {
            _controller = new MainController();
        }

        [Fact]
        public void ShouldCreate()
        {
            _controller.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Return_Message()
        {
            // Arrange

            // Setup

            // Act

            var result = _controller.Get() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Value.Should().Be("Fullstack Challenge 20201026");
        }
    }
}
