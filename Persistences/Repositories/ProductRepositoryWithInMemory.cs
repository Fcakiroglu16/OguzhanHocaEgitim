using Applications.Products;
using Domains;

namespace Persistences.Repositories
{
    public class ProductRepositoryWithInMemory : IProductRepository
    {
        private static readonly List<Product> Products =
        [
            new Product() { Id = 1, Name = "kalem 1", Price = 100, Barcode = "ABCDEF" },
            new Product() { Id = 2, Name = "kalem 2", Price = 200, Barcode = "GHIJKL" }
        ];

        public List<Product> GetAll()
        {
            return Products;
        }

        public List<Product> GeatAllByPaged(int page, int pageSize)
        {
            //1. senaryo=>  page=1, pageSize=10 => 

            return Products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public void Update(Product product)
        {
            var index = Products.FindIndex(p => p.Id == product.Id);
            Products[index] = product;
        }

        public Product Create(Product product)
        {
            product.Id = Products.Count != 0 ? Products.Max(p => p.Id) + 1 : 1;
            Products.Add(product);
            return product;
        }

        public Product? Get(int id)
        {
            return Products.FirstOrDefault(p => p.Id == id);
        }

        public bool Exist(string productName)
        {
            return Products.Any(p => p.Name == productName);
        }

        public void Delete(Product product)
        {
            Products.Remove(product);
        }
    }
}