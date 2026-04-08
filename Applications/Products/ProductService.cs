using System.Net;
using Applications.Products.Create;
using Applications.Products.Dto;
using Applications.Products.Update;
using Domains;

namespace Applications.Products
{
    public class ProductService(IProductRepository productRepository, TaxCalculate taxCalculate) : IProductService
    {
        public const int BarcodeLength = 6;


        public ServiceResult<List<ProductDto>> GetAll()
        {
            //success =>  empty ( Status Code )
            //success =>  list of products  ( Status Code )
            //failure => empty  ( Status Code )
            //failure => error list  ( Status Code )


            //Result Pattern =>

            var productList = productRepository.GetAll();

            var productListAsDto = new List<ProductDto>();

            foreach (var product in productList)
            {
                var productDto = new ProductDto(product.Id, product.Name, taxCalculate.CalculateTax(product.Price, 20));
                productListAsDto.Add(productDto);
            }


            return ServiceResult<List<ProductDto>>.Success(productListAsDto, HttpStatusCode.OK);

            //return new ServiceResult<List<ProductDto>>()
            //{
            //    Data = productListAsDto,
            //    StatusCode = HttpStatusCode.OK
            //};
        }


        public ServiceResult<List<ProductDto>> GetAllByPaged(int page, int pageSize)
        {
            var pagedProductList = productRepository.GeatAllByPaged(page, pageSize);
            var pagedProductListAsDto = pagedProductList
                .Select(product =>
                    new ProductDto(product.Id, product.Name, taxCalculate.CalculateTax(product.Price, 20))).ToList();


            // LinQ => Language Integrated Query


            return ServiceResult<List<ProductDto>>.Success(pagedProductListAsDto, HttpStatusCode.OK);
        }


        public ServiceResult<ProductDto> GetById(int id)
        {
            var product = productRepository.Get(id);
            if (product is null)
            {
                return ServiceResult<ProductDto>.Failure(HttpStatusCode.NotFound, $"Product with id {id} not found.");
            }

            var productAsDto = new ProductDto(product.Id, product.Name, taxCalculate.CalculateTax(product.Price, 20));
            return ServiceResult<ProductDto>.Success(productAsDto, HttpStatusCode.OK);
        }


        public ServiceResult Update(UpdateProductRequest request)
        {
            var hasProduct = productRepository.Get(request.Id);

            if (hasProduct is null)
            {
                return ServiceResult.Failure(HttpStatusCode.NotFound, "Product with id {request.Id} not found.");
            }

            hasProduct.Name = request.Name;
            hasProduct.Price = request.Price;


            productRepository.Update(hasProduct);


            return ServiceResult.Success(HttpStatusCode.NoContent);
            //return new ServiceResult()
            //{
            //    StatusCode = HttpStatusCode.NoContent
            //};
        }

        public ServiceResult<CreateProductResponse> Create(CreateProductRequest request)
        {
            var existProduct = productRepository.Exist(request.Name);

            if (existProduct)
            {
                return ServiceResult<CreateProductResponse>.Failure(HttpStatusCode.BadRequest,
                    $"Product with name {request.Name} already exist.");
            }

            var newProduct = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Barcode = GenerateBarcode()
            };

            var createdProduct = productRepository.Create(newProduct);

            return ServiceResult<CreateProductResponse>.Success(new CreateProductResponse(createdProduct.Id),
                HttpStatusCode.Created);
        }

        public ServiceResult Delete(int id)
        {
            var hasProduct = productRepository.Get(id);

            if (hasProduct is null)
            {
                return ServiceResult.Failure(HttpStatusCode.NotFound, "Product with id {request.Id} not found.");
            }


            productRepository.Delete(hasProduct);

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }


        private static string GenerateBarcode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = Random.Shared;

            return string.Create(BarcodeLength, random, (span, rng) =>
            {
                for (var i = 0; i < span.Length; i++)
                {
                    span[i] = chars[rng.Next(chars.Length)];
                }
            });
        }
    }
}