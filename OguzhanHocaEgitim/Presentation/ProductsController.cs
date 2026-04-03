using System.Net;
using Applications;
using Applications.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OguzhanHocaEgitim.ApplicationsServices;

namespace OguzhanHocaEgitim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProducts()
        {
            var result = productService.GetProducts();


            if (result.StatusCode == HttpStatusCode.NoContent)
            {
                return new ObjectResult(null)
                {
                    StatusCode = result.StatusCode.GetHashCode()
                };
            }

            if (result.IsSuccess)
            {
                return new ObjectResult(result.Data)
                {
                    StatusCode = result.StatusCode.GetHashCode()
                };
            }


            return new ObjectResult(result.Errors)
                    {
                        StatusCode = result.StatusCode.GetHashCode()
                    };
        }

        //[HttpGet]
        //public IActionResult GetProductsWithPaged()
        //{
        //    return Ok("products");
        //}


        [HttpPost]
        public IActionResult CreateProduct()
        {
            // 200 OK => İstek başarılı oldu ve sonuç döndürüldü.
            // 201 Created => Yeni bir kaynak oluşturuldu.
            return Created(new Uri("https://products"), null);
        }

        [HttpPut]
        public IActionResult UpdateProduct()
        {
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteProduct()
        {
            return NoContent();
        }

        [HttpPatch]
        public IActionResult UpdateProductName()
        {
            return NoContent();
        }
    }
}
