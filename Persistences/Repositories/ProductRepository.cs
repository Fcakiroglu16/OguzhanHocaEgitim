using System;
using System.Collections.Generic;
using System.Text;
using Applications.Products;
using Domains;
using Microsoft.EntityFrameworkCore;

namespace Persistences.Repositories
{
    public class ProductRepository(AppDbContext context) : GenericRepository<Product>(context), IProductRepository
    {
        public List<Product> GeatAllByPaged(int page, int pageSize)
        {
            // 1,10 => Skip(0).Take(10)
            // 2,10 => Skip(10).Take(10)

            return context.Products.AsNoTracking().Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Product> Get(decimal price)
        {
            return context.Products.Where(x => x.Price == price).ToList();
        }
    }
}
