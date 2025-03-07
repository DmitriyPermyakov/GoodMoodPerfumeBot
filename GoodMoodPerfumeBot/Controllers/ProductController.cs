using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodMoodPerfumeBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok("hello");
        }
    }
}
