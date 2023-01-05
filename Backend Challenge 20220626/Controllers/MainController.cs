using Microsoft.AspNetCore.Mvc;

namespace Backend_Challenge_20220626.Controllers
{
    [ApiController]
    [Route("")]
    public class MainController
    {
        /// <summary>
        /// Show a simple message
        /// </summary>
        /// <returns>Fullstack Challenge 20201026</returns>
        /// <response code="200">Fullstack Challenge 20201026</response>
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        [HttpGet]
        public IActionResult Get() => new OkObjectResult("Fullstack Challenge 20201026");
    }
}
