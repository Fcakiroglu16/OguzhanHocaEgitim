using Applications.ActivitySource;
using Applications.Products.Create;
using Applications.Products.Dto;
using Applications.Products.Update;
using Domains;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;

namespace Applications.Products
{
    public class ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        TaxCalculate taxCalculate,
        IValidator<CreateProductRequest> createProductRequestValidator,
        ILogger<ProductService> logger,
        ILoggerFactory loggerFactory) : IProductService
    {
        public const int BarcodeLength = 6;


        public ServiceResult<List<ProductDto>> GetAll()
        {
            logger.LogInformation("GetAll methodu çalıştı");


            var logger2 = loggerFactory.CreateLogger("xxxx");

            logger2.LogInformation("GetAll methodu çalıştı (logger2)");


            var productList = productRepository.GetAll();

            var productListAsDto = new List<ProductDto>();

            var userId = 100;
            var tenantId = 500;
            using (var activity =
                   ActivitySourceProvider.ActivitySource.StartActivity("product_list", ActivityKind.Server))
            {
                activity!.AddTag("userId", userId.ToString());
                activity!.AddTag("tenantId", tenantId.ToString());

                activity.AddEvent(new ActivityEvent("product list datası çekilmeden önce"));

                productListAsDto.AddRange(productList.Select(product =>
                    new ProductDto(product.Id, product.Name, taxCalculate.CalculateTax(product.Price, 20))));

                activity.AddEvent(new ActivityEvent("product list datası çekildikten sonra"));
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


            unitOfWork.Commit();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }


        public record CreateProductAndCategoryRequest(string Name, decimal Price, string CategoryName);


        public ServiceResult CreateWithCategory(CreateProductAndCategoryRequest request)
        {
            var hasCategory = categoryRepository.Exist(request.CategoryName);

            if (hasCategory)
            {
                return ServiceResult.Failure(HttpStatusCode.BadRequest, "kategori ismi veritabanında bulunmaktadır.");
            }

            unitOfWork.BeginTransaction();
            var category = categoryRepository.Create(new Category() { Name = request.Name });


            unitOfWork.Commit();

            var product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                CategoryId = category.Id
            };

            productRepository.Create(product);
            unitOfWork.Commit();


            unitOfWork.CommitTransaction();
            return ServiceResult.Success(HttpStatusCode.Created);
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
                Price = request.Price!.Value,
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


            productRepository.Remove(hasProduct);


            unitOfWork.Commit();
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