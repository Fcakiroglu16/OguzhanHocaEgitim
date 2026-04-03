using System.Net;
using Applications.Products.Dto;
using Applications.Products.Update;

namespace Applications.Products
{
    public class ProductService(IProductRepository productRepository)
    {
        public ServiceResult<List<ProductDto>> GetProducts()
        {
            //success =>  empty ( Status Code )
            //success =>  list of products  ( Status Code )
            //failure => empty  ( Status Code )
            //failure => error list  ( Status Code )


            //Result Pattern =>

            var productList = productRepository.GetAllProducts();

            var productListAsDto = new List<ProductDto>();

            foreach (var product in productList)
            {
                var productDto = new ProductDto(product.Id, product.Name, product.Price);
                productListAsDto.Add(productDto);
            }


            return new ServiceResult<List<ProductDto>>()
            {
                Data = productListAsDto,
                StatusCode = HttpStatusCode.OK
            };
        }

        public ServiceResult UpdateProduct(UpdateProductRequest request)
        {
            var hasProduct = productRepository.GetProduct(request.Id);

            if (hasProduct is null)
            {
                return new ServiceResult()
                {
                    Errors = [$"Product with id {request.Id} not found."],
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            hasProduct.Name = request.Name;
            hasProduct.Price = request.Price;


            productRepository.UpdateProduct(hasProduct);

            return new ServiceResult()
            {
                StatusCode = HttpStatusCode.NoContent
            };
        }
    }
}