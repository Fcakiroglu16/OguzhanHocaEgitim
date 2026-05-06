using System;
using System.Collections.Generic;
using System.Text;
using Applications;
using Domains;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Persistences
{
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category>(context), ICategoryRepository
    {
        //Explicit Loading
        public Category GetByIdWithProductAndFeature(int id, bool isReference)
        {
            var category = Context.Categories.First(x => x.Id == id);


            if (true)
            {
                var products = Context.Products.Where(p => p.CategoryId == category.Id).ToList();
            }


            if (isReference)
            {
                Context.Entry(category).Collection(x => x.Products!).Load();


                if (category.Products is not null)
                {
                    foreach (var product in category.Products)
                    {
                        Context.Entry(product).Reference(x => x.ProductDetail).Load();
                    }
                }
            }

            return category;
        }

        //Lazy Loading
        public Category GetByIdWithProductAndFeature2(int id, bool isReference)
        {
            var category = Context.Categories.First(x => x.Id == id);

            var products = category.Products.ToList();


            return category;
        }
    }
}