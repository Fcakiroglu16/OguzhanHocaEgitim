namespace Domains
{
    //  object = data + behavior

    public class A
    {
        public A()
        {
            var product = new Product();
            product.Id = 1;
            product.Price = 200;
        }
    }


    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public decimal Price { get; private set; }

        public string Barcode { get; set; } = null!;


        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
            {
                throw new Exception("Price cannot be negative.");
            }

            Price = newPrice;
        }
    }
}