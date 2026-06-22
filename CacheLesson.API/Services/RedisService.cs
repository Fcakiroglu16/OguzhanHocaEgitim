using StackExchange.Redis;

namespace CacheLesson.API.Services
{
    public class RedisService(IConfiguration configuration)
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer =
            ConnectionMultiplexer.Connect($"{configuration.GetConnectionString("Redis")}");


        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.GetDatabase(1);
        }
    }
}
