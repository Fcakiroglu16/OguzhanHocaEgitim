using System;
using System.Collections.Generic;
using System.Text;
using Domains;

namespace Applications.Products
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public List<Product> GeatAllByPaged(int page, int pageSize);

        public List<Product> Get(decimal price);
    }
}
