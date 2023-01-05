using Application.Products;
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

        [HttpGet]
        [Route("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var product = await _productService.GetAsync(code);

            if (product is null) return new NotFoundObjectResult("Product with this code not founded!");

            return new OkObjectResult(product);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(int page, int pageSize) => new OkObjectResult(await _productService.GetPaged(page,pageSize));
    }
}
