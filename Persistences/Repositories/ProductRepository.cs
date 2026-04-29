using System;
using System.Collections.Generic;
using System.Text;
using Applications.Products;
using Domains;
using Microsoft.EntityFrameworkCore;

namespace Persistences.Repositories
{
    internal class ProductRepository(AppDbContext context) : IProductRepository
    {
        public List<Product> GetAll()
        {
            return context.Products.AsNoTracking().ToList();
        }

        public List<Product> GeatAllByPaged(int page, int pageSize)
        {
            // 1,10 => Skip(0).Take(10)
            // 2,10 => Skip(10).Take(10)

            return context.Products.AsNoTracking().Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public void Update(Product product)
        {
            //  context.Entry(product).State = EntityState.Modified;

            context.Products.Update(product);
        }

        public Product Create(Product product)
        {
            // context.Entry(product).State = EntityState.Added;
            context.Products.Add(product);
            return product;
        }

        public Product? Get(int id)
        {
            var product = context.Products.Find(id);


            var state = context.Entry(product).State;

            return product;
        }

        public bool Exist(string productName)
        {
            return context.Products.Any(p => p.Name == productName);
        }

        public void Remove(Product product)
        {
            //context.Entry(product).State = EntityState.Deleted;
            context.Products.Remove(product);
        }
    }
}
