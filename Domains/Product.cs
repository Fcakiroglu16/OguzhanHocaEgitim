using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domains.Exceptions;

namespace Domains
{
    //  object = data + behavior
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
        int UserId { get; set; }
    }

    public class Product : IAuditable
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public decimal Price { get; set; }

        public string Barcode { get; set; } = null!;

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;

        public virtual ProductDetail? ProductDetail { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int UserId { get; set; }
    }
}