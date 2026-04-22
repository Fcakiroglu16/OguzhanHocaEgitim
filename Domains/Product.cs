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


        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
            {
                throw new BusinessException("Price cannot be negative.");
            }

            Price = newPrice;
        }
    }
}