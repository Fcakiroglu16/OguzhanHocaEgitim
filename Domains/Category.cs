using System;
using System.Collections.Generic;
using System.Text;

namespace Domains
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        //navigation property
        public List<Product>? Products { get; set; }
    }
}
