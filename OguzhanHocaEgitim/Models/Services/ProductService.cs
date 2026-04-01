using OguzhanHocaEgitim.Models.Repositories;

namespace OguzhanHocaEgitim.Models.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService()
        {
            _productRepository = new ProductRepository();
        }


        public List<ProductDto> GetProducts()
        {
            var productList = _productRepository.GetAllProducts();

            var productListAsDto = new List<ProductDto>();

            foreach (var product in productList)
            {
                var productDto = new ProductDto(product.Id, product.Name, product.Price);
                productListAsDto.Add(productDto);
            }

            return productListAsDto;
        }
    }
}
