using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Get(string categoryName, int categoryId)
        {
            return Ok();
        }
    }
}
