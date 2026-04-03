using Applications.Products;
using OguzhanHocaEgitim.Domains;

namespace Persistences.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static readonly List<Product> Products =
        [
            new Product() { Id = 1, Name = "kalem 1", Price = 100, Barcode = "abc" },
            new Product() { Id = 1, Name = "kalem 1", Price = 100, Barcode = "abc" }
        ];


        public List<Product> GetAllProducts()
        {
            return Products;
        }
    }
}