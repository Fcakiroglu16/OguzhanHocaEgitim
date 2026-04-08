using System;
using System.Collections.Generic;
using System.Text;
using Domains;

namespace Applications.Products
{
    public interface IProductRepository
    {
        public List<Product> GetAll();

        public List<Product> GeatAllByPaged(int page, int pageSize);

        public void Update(Product product);

        public Product Create(Product product);
        public Product? Get(int id);

        public bool Exist(string productName);
        void Delete(Product product);
    }
}
