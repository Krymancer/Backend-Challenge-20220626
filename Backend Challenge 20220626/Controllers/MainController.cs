using Microsoft.AspNetCore.Mvc;

namespace Backend_Challenge_20220626.Controllers
{
    [ApiController]
    [Route("")]
    public class MainController
    {
        [HttpGet]
        public IActionResult Get() => new OkObjectResult("Fullstack Challenge 20201026");
    }
}
