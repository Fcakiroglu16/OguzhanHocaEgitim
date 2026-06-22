namespace CacheLesson.API
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }

    public class ProductRepository
    {
        private static readonly List<Product> _products =
        [
            new Product { Id = 1, Name = "Laptop", Price = 999.99m },
            new Product { Id = 2, Name = "Smartphone", Price = 499.99m },
            new Product { Id = 3, Name = "Headphones", Price = 199.99m }
        ];

        public Task<List<Product>> GetProductsAsync()
        {
            // Simulate a delay to mimic database access
            return Task.FromResult(_products);
        }

        public Task Add(Product product)
        {
            _products.Add(product);
            return Task.CompletedTask;
        }
    }
}
