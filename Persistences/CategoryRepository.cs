using System;
using System.Collections.Generic;
using System.Text;
using Applications;
using Domains;

namespace Persistences
{
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category>(context), ICategoryRepository
    {
    }
}
