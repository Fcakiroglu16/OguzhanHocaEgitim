using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace CacheLesson.API;

public class ProductServiceAsDistributedCache(ProductRepository repository, IDistributedCache distributedCache)
{
    public async Task<List<Product>> GetProductsAsync()
    {
        var productsAsJson = await distributedCache.GetStringAsync("products");

        List<Product> products;


        if (!string.IsNullOrEmpty(productsAsJson))
        {
            products = JsonSerializer.Deserialize<List<Product>>(productsAsJson!)!;

            return products;
        }


        var productsFromDb = await repository.GetProductsAsync();


        productsAsJson = JsonSerializer.Serialize(productsFromDb);

        var cacheOptions = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(5),
        };

        await distributedCache.SetStringAsync("products", productsAsJson, cacheOptions);


        return productsFromDb;
    }

    public async Task AddProductAsync(Product product)
    {
        await repository.Add(product);
        await distributedCache.RemoveAsync("products");
    }
}