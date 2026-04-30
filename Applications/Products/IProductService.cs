using Applications.Products.Create;
using Applications.Products.Dto;
using Applications.Products.Update;
using System;
using System.Collections.Generic;
using System.Text;
using static Applications.Products.ProductService;

namespace Applications.Products
{
    public interface IProductService
    {
        ServiceResult<List<ProductDto>> GetAll();
        ServiceResult<List<ProductDto>> GetAllByPaged(int page, int pageSize);
        ServiceResult<ProductDto> GetById(int id);
        ServiceResult Update(UpdateProductRequest request);
        ServiceResult<CreateProductResponse> Create(CreateProductRequest request);
        ServiceResult Delete(int id);
        ServiceResult CreateWithCategory(CreateProductAndCategoryRequest request);
    }
}
