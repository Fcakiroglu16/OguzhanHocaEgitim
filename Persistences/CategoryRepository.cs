using System;
using System.Collections.Generic;
using System.Text;
using Applications;
using Domains;

namespace Persistences
{
    public class CategoryRepository(AppDbContext context) : ICategoryRepository
    {
        public Category Create(Category category)
        {
            context.Categories.Add(category);
            return category;
        }

        public bool Exist(string categoryName)
        {
            return context.Categories.Any(c => c.Name == categoryName);
        }
    }
}
