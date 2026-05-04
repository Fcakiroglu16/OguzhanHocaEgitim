using System;
using System.Collections.Generic;
using System.Text;

namespace Domains
{
    public enum ProductColor : byte
    {
        Red = 1,
        Blue = 2,
        Green = 3,
    }


    public class ProductDetail
    {
        public int Height { get; set; }

        public int Width { get; set; }

        public ProductColor Color { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;
    }
}
