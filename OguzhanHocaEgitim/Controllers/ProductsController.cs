using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OguzhanHocaEgitim.Models.Services;

namespace OguzhanHocaEgitim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ProductService _productService = new ProductService();

        //public methods => endpoints ( Request  Method Type(GET,POST, PUT, DELETE) ) + Respones (Status Code + Data )

        //  Business Logic / DAL Logic


        //[HttpGet]
        //public IActionResult GetProducts()
        //{

        //    return Ok(_productService.GetProducts());
        //}
        public IActionResult GetProducts() => Ok(_productService.GetProducts());

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
