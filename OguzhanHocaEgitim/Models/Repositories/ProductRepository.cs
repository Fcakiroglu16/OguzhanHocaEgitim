namespace OguzhanHocaEgitim.Models.Repositories
{
    public class ProductRepository
    {
        private static readonly List<Product> _products =
        [
            new Product() { Id = 1, Name = "kalem 1", Price = 100, Barcode = "abc" },
            new Product() { Id = 1, Name = "kalem 1", Price = 100, Barcode = "abc" }
        ];


        public List<Product> GetAllProducts()
        {
            return _products;
        }
    }
}
