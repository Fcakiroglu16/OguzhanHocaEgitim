using System.Net;
using Applications.Products;
using Applications.Products.Create;
using Applications.Products.Update;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Presentation
{
    public class ProductsController(IProductService productService) : CustomBaseController
    {
        [HttpGet]
        public IActionResult GetProducts() => CreateActionResult(productService.GetAll());

        [HttpGet]
        public IActionResult GetProductById(int id)
        {
            return CreateActionResult(productService.GetById(id));
        }

        //[HttpGet]
        //public IActionResult UpdateProductName(int page, int pageSize)
        //{
        //    return CreateActionResult(productService.GetAllByPaged(page, pageSize));
        //}


        [HttpPost]
        public IActionResult CreateProduct(CreateProductRequest request)
        {
            return CreateActionResult(productService.Create(request));
        }

        [HttpPut]
        public IActionResult UpdateProduct(UpdateProductRequest request)
        {
            return CreateActionResult(productService.Update(request));
        }

        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            return CreateActionResult(productService.Delete(id));
        }
    }
}
