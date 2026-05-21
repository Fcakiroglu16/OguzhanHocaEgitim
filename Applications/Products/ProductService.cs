using Applications.ActivitySource;
using Applications.Products.Create;
using Applications.Products.Dto;
using Applications.Products.Update;
using Domains;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Drawing;
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
        public record GetAllWithCategoryAndFeatureResponse(
            int Id,
            string Name,
            decimal Price,
            string CategoryName,
            int? Width,
            int? Height);

        public ServiceResult<List<GetAllWithCategoryAndFeatureResponse>> GetAllWithCategoryAndFeature()
        {
            var products = productRepository.GetAllWithCategoryAndFeature();


            var productsAsDto = products.Select(p => new GetAllWithCategoryAndFeatureResponse(
                p.Id,
                p.Name,
                taxCalculate.CalculateTax(p.Price, 20),
                p.Category.Name,
                p.ProductDetail?.Width,
                p.ProductDetail?.Height)).ToList();


            return ServiceResult<List<GetAllWithCategoryAndFeatureResponse>>.Success(productsAsDto, HttpStatusCode.OK);
        }


        public ServiceResult<List<ProductDto>> GetAll()
        {
            logger.LogInformation("Fetching all products.");


            // var SpProductResult = productRepository.StoreProcedureExample();


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
            var product = productRepository.GetById(id);
            if (product is null)
            {
                return ServiceResult<ProductDto>.Failure(HttpStatusCode.NotFound, $"Product with id {id} not found.");
            }

            var productAsDto = new ProductDto(product.Id, product.Name, taxCalculate.CalculateTax(product.Price, 20));
            return ServiceResult<ProductDto>.Success(productAsDto, HttpStatusCode.OK);
        }


        public ServiceResult Update(UpdateProductRequest request)
        {
            var hasProduct = productRepository.GetById(request.Id);

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


        public record CreateProductAndCategoryAndDetailsRequest(
            string Name,
            decimal Price,
            string CategoryName,
            int Width,
            int Height,
            ProductColor color);


        public ServiceResult CreateWithCategoryAndDetails(CreateProductAndCategoryAndDetailsRequest request)
        {
            var hasCategory = categoryRepository.Where(c => c.Name == request.CategoryName).FirstOrDefault();


            var product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Barcode = "AAAAAAAAAA",
                ProductDetail = new ProductDetail()
                {
                    Height = request.Height,
                    Width = request.Width,
                    Color = request.color,
                }
            };


            if (hasCategory is null)
            {
                hasCategory = new Category
                {
                    Name = request.CategoryName,
                    Products = [product]
                };

                categoryRepository.Add(hasCategory);
            }
            else
            {
                hasCategory.Products = [product];

                categoryRepository.Update(hasCategory);
            }

            unitOfWork.Commit();
            return ServiceResult.Success(HttpStatusCode.Created);
        }


        public ServiceResult<CreateProductResponse> Create(CreateProductRequest request)
        {
            #region Insert Store Procedure Example

            //var newProduct = productRepository.StoreProcedureInsertExample(request.Name, request.Price!.Value,
            //    GenerateBarcode(), request.CategoryId);


            //return ServiceResult<CreateProductResponse>.Success(new CreateProductResponse(newProduct.Id),
            //    HttpStatusCode.Created); 

            #endregion


            var existProduct = productRepository.Exist(p => p.Name == request.Name);


            if (existProduct)
            {
                return ServiceResult<CreateProductResponse>.Failure(HttpStatusCode.BadRequest,
                    $"Product with name {request.Name} already exist.");
            }

            var newProduct = new Product()
            {
                Name = request.Name,
                Price = request.Price!.Value,
                Barcode = GenerateBarcode(),
                CategoryId = request.CategoryId
            };

            var createdProduct = productRepository.Add(newProduct);


            unitOfWork.Commit();
            return ServiceResult<CreateProductResponse>.Success(new CreateProductResponse(createdProduct.Id),
                HttpStatusCode.Created);
        }

        public ServiceResult<CreateProductResponse> Create2(CreateProductRequest request)
        {
            var category = categoryRepository.Where(x => x.Id == request.CategoryId).FirstOrDefault();


            if (category is null)
            {
                return ServiceResult<CreateProductResponse>.Failure(HttpStatusCode.BadRequest,
                    $"Category with id {request.CategoryId} not found.");
            }

            var newProduct = new Product()
            {
                Name = request.Name,
                Price = request.Price!.Value,
                Barcode = GenerateBarcode()
            };

            category.Products =
            [
                newProduct
            ];

            categoryRepository.Update(category);

            unitOfWork.Commit();


            return ServiceResult<CreateProductResponse>.Success(new CreateProductResponse(newProduct.Id),
                HttpStatusCode.Created);
        }

        public ServiceResult Delete(int id)
        {
            var hasProduct = productRepository.GetById(id);

            if (hasProduct is null)
            {
                return ServiceResult.Failure(HttpStatusCode.NotFound, "Product with id {request.Id} not found.");
            }


            productRepository.Delete(hasProduct);


            unitOfWork.Commit();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        private static string GenerateBarcode()
        {
            const int BarcodeLength = 10;
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