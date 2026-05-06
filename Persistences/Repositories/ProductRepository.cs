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
        //Eager Loading
        public Product? GetByIdWithCategoryAndFeature(int id)
        {
            return Context.Products.Include(p => p.Category).Include(p => p.ProductDetail)
                .FirstOrDefault(x => x.Id == id);
        }

        //Eager Loading
        public List<Product> GetAllWithCategoryAndFeature()
        {
            var categories = Context.Categories.Include(c => c.Products)!.ThenInclude(p => p.ProductDetail).ToList();


            //Eager Loading
            return Context.Products.Include(p => p.Category).Include(p => p.ProductDetail).ToList();
        }


        //Explicit Loading
        public Product? ExplicitLoading(int id, bool isReference)
        {
            var product = Context.Products.First(x => x.Id == id);


            if (!isReference) return product;

            Context.Entry(product).Reference(p => p.Category).Load();

            Context.Entry(product).Reference(p => p.ProductDetail).Load();


            return product;
        }

        //lazy Loading
        public List<Product> LazyLoading()
        {
            var products = context.Products.ToList();
            foreach (Product product in products)
            {
                var category = product.Category;
                var Detail = product.ProductDetail;
            }

            return products;
        }

        public List<Product> GeatAllByPaged(int page, int pageSize)
        {
            // 1,10 => Skip(0).Take(10)
            // 2,10 => Skip(10).Take(10)

            return Context.Products.AsNoTracking().Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Product> Get(decimal price)
        {
            return Context.Products.Where(x => x.Price == price).ToList();
        }
    }
}
