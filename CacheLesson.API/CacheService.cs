using System.Text.Json;
using CacheLesson.API.Services;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using StackExchange.Redis;

namespace CacheLesson.API
{
    public class CacheService(RedisService redisService)
    {
        private readonly IDatabase _db = redisService.GetDatabase();

        public async Task SetString(string key, string value)
        {
            await _db.StringSetAsync(key, value, TimeSpan.FromMinutes(5), When.Always);
        }

        public async Task SetString<T>(string key, T value)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, TimeSpan.FromMinutes(5), When.Always);
        }


        public async Task<T> GetString<T>(string key)
        {
            var json = await _db.StringGetAsync(key);
            if (json.IsNullOrEmpty)
            {
                return default!;
            }

            return System.Text.Json.JsonSerializer.Deserialize<T>(json.ToString())!;
        }


        public async Task<string?> GetString(string key)
        {
            return await _db.StringGetAsync(key);
        }


        public async Task AddList(string key, string item)
        {
            //LinkedList<string> names = new LinkedList<string>();
            //var ahmetNode=  names.AddFirst("ahmet");
            //names.AddFirst("mehmet");
            //names.AddAfter(ahmetNode, "hasan");


            //await _db.ListInsertAfterAsync(key, item, item);

            await _db.KeyExpireAsync(key, TimeSpan.FromMinutes(5));
            await _db.ListRightPushAsync(key, item);
        }

        public async Task RemoveList(string key, string item)
        {
            await _db.ListRemoveAsync(key, item);
        }


        public async Task AddSet(string key, string item)
        {
            await _db.SetAddAsync(key, item);
        }

        public async Task RemoveSet(string key, string item)
        {
            await _db.SetRemoveAsync(key, item);
        }

        public async Task AddSortedSet(string key, string item, double score)
        {
            string name = "ahmet";


            await _db.SortedSetAddAsync(key, item, score);
        }

        public async Task RemoveSortedSet(string key, string item)
        {
            await _db.SortedSetRemoveAsync(key, item);
        }

        public async Task<List<string>> GetSortedList(string key)
        {
            var list = await _db.SortedSetRangeByScoreAsync(key, 1, 100, Exclude.None, Order.Descending, 0, -1);
            return list.Select(x => x.ToString()).ToList();
        }

        public async Task AddHash(string key, string field, string value)
        {
            await _db.HashSetAsync(key, field, value);
        }

        public async Task RemoveHash(string key, string field)
        {
            await _db.HashDeleteAsync(key, field);
        }

        public async Task<string?> GetHash(string key, string field)
        {
            return await _db.HashGetAsync(key, field);
        }


        public async Task AddProductList()
        {
            var producsRepository = new ProductRepository();

            var products = await producsRepository.GetProductsAsync();

            foreach (var product in products)
            {
                _db.ListRightPush("products", System.Text.Json.JsonSerializer.Serialize(product));
            }
        }

        public async Task AddProductHash()
        {
            var producsRepository = new ProductRepository();

            var products = await producsRepository.GetProductsAsync();

            foreach (var product in products)
            {
                _db.HashSet("products", product.Id.ToString(), System.Text.Json.JsonSerializer.Serialize(product));
            }
        }

        public async Task<Product?> GetProduct(int id)
        {
            var productAsString = _db.HashGet("products", id.ToString());

            return JsonSerializer.Deserialize<Product>(productAsString.ToString());
        }

        public async Task AddUserToHash()
        {
            _db.HashSet("user:1500", "name", "John Doe");
            _db.HashSet("user:1500", "email", "john.doe@example.com");
        }
    }
}
