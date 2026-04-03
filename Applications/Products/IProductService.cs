using Applications.Products.Dto;
using Applications.Products.Update;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Products
{
    public interface IProductService
    {
        ServiceResult<List<ProductDto>> GetProducts();
        ServiceResult UpdateProduct(UpdateProductRequest request);
    }
}
