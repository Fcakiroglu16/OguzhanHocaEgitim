using Microsoft.Extensions.Caching.Memory;

namespace CacheLesson.API
{
    public class ProductServiceAsInMemoryCache(ProductRepository repository, IMemoryCache memoryCache)
    {
        public bool Any(out string key)
        {
            key = "products";
            return true;
        }


        public async Task<List<Product>> GetProductsAsync()
        {
            //cache aside design pattern
            if (memoryCache.TryGetValue("products", out List<Product> products))
            {
                return products;
            }

            var productsFromDb = await repository.GetProductsAsync();


            //memoryCache.Set("products", product);

            //memoryCache.Set("products", product, TimeSpan.FromSeconds(10));


            var memoryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                SlidingExpiration = TimeSpan.FromMinutes(5),
                Priority = CacheItemPriority.High,
            };

            memoryCache.Set("products", productsFromDb, memoryOptions);


            return productsFromDb;
        }

        public async Task AddProductAsync(Product product)
        {
            await repository.Add(product);


            // 1.yol Invalidate the cache
            memoryCache.Remove("products");

            // 2.yol Update the cache
            //var productsFromDb = await repository.GetProductsAsync();
            //var memoryOptions = new MemoryCacheEntryOptions()
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            //    SlidingExpiration = TimeSpan.FromMinutes(5),
            //    Priority = CacheItemPriority.High,
            //};

            //memoryCache.Set("products", productsFromDb, memoryOptions);
        }
    }
}
