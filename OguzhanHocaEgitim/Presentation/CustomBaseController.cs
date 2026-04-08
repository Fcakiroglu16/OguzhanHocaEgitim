using Applications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        protected IActionResult CreateActionResult(ServiceResult result)
        {
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return new ObjectResult(result.ProblemDetails)
            {
                StatusCode = result.StatusCode.GetHashCode()
            };
        }


        protected IActionResult CreateActionResult<T>(ServiceResult<T> result)
        {
            if (result.IsSuccess)
            {
                return new ObjectResult(result.Data)
                {
                    StatusCode = result.StatusCode.GetHashCode()
                };
            }

            return new ObjectResult(result.ProblemDetails)
            {
                StatusCode = result.StatusCode.GetHashCode()
            };
        }
    }
}
