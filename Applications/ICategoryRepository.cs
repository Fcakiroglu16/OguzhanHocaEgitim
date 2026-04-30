using System;
using System.Collections.Generic;
using System.Text;
using Domains;

namespace Applications
{
    public interface ICategoryRepository
    {
        Category Create(Category category);

        public bool Exist(string categoryName);
    }
}
