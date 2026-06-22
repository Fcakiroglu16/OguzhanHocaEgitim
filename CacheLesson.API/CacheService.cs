using CacheLesson.API.Services;
using StackExchange.Redis;

namespace CacheLesson.API
{
    public class CacheService(RedisService redisService)
    {
        private readonly IDatabase _db = redisService.GetDatabase();

        public async Task SetString(string key, string value)
        {
            await _db.StringSetAsync(key, value);
        }

        public async Task<string?> GetString(string key)
        {
            return await _db.StringGetAsync(key);
        }
    }
}
