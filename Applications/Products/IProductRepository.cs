using System;
using System.Collections.Generic;
using System.Text;
using OguzhanHocaEgitim.Domains;

namespace Applications.Products
{
    public interface IProductRepository
    {
        public List<Product> GetAllProducts();

        public void UpdateProduct(Product product);

        public Product? GetProduct(int id);
    }
}
