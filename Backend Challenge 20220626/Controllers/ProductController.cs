using System.Net;
using Application.Products;
using Domain.Products;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Challenge_20220626.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Retrieve a product from database
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Product referring to the code provided</returns>
        /// <response code="200">Product referring to the code provided</response>
        /// <response code="404">Unable to find product with provided code</response>
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var product = await _productService.GetByCodeAsync(code);

            if (product is null) return new NotFoundObjectResult("Product with this code not founded!");

            return new OkObjectResult(product);
        }

        /// <summary>
        /// List products in database
        /// </summary>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Number of results per page</param>
        /// <returns>A list of products</returns>
        /// <response code="200">List the products accordingly with the page and page size parameters</response>
        /// <response code="400">If the current page is negative</response>
        [ProducesResponseType(typeof(List<Product>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(int page, int pageSize) {
            if (page < 0) return new BadRequestObjectResult("Page must be a positive integer");
        
            return new OkObjectResult(await _productService.GetPaged(page, pageSize));
        }
    }
}
