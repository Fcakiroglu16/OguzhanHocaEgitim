using System.Net;
using Applications.Products;
using Applications.Products.Create;
using Applications.Products.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Presentation
{
    public class ProductsController(IProductService productService) : CustomBaseController
    {
        // simple type => int, string, bool, decimal, datetime => querystring[default],route
        // complex type => class, record, struct => body[default]
        // querystring // api/products/getproductbyid?id=1
        // route // api/products/getproductbyid/1
        // body => postman => raw => json
        // header =>  key value pair => Authorization: Bearer token
        // form data =>  postman => form-data => key value pair => file, text

        // api/products
        [HttpGet]
        public IActionResult GetProducts() => CreateActionResult(productService.GetAll());


        // api/products?id=1
        [HttpGet("{id}")]
        public IActionResult GetProductById([FromRoute] int id)
        {
            return CreateActionResult(productService.GetById(id));
        }

        //api/products/getallbypaged?page=1&pageSize=10
        [HttpGet("{page}/{pageSize}")]
        public IActionResult GetAllByPaged([FromRoute] int page, [FromRoute] int pageSize)
        {
            return CreateActionResult(productService.GetAllByPaged(page, pageSize));
        }


        //api/products/getallbypaged?page=1&pageSize=10
        [HttpGet("page/{page}/pagesize/{pageSize}")]
        public IActionResult GetAllByPaged2([FromRoute] int page, [FromRoute] int pageSize)
        {
            return CreateActionResult(productService.GetAllByPaged(page, pageSize));
        }


        [HttpPost]
        public IActionResult CreateProduct([FromBody] CreateProductRequest request)
        {
            return CreateActionResult(productService.Create(request));
        }

        [HttpPut]
        public IActionResult UpdateProduct([FromBody] UpdateProductRequest request)
        {
            return CreateActionResult(productService.Update(request));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct([FromRoute] int id)
        {
            return CreateActionResult(productService.Delete(id));
        }
    }
}
