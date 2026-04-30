using System.ComponentModel.DataAnnotations;
using Domains.Exceptions;

namespace Domains
{
    //  object = data + behavior


    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public decimal Price { get; set; }

        public string Barcode { get; set; } = null!;


        public int CategoryId { get; set; }
    }
}