using System;
using System.Collections.Generic;
using System.Text;
using Applications.Products;
using Domains;

namespace Applications
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
    }
}
