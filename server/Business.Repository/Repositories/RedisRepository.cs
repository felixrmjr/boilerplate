using Business.Domain.Interfaces.Repositories;
using Business.Domain.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Business.Repository.Repositories
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly RedisConnection _configRedis;
        private readonly ConnectionMultiplexer connectionRedis;
        private readonly IDatabase clientRedis;

        public RedisRepository(IConnectionMultiplexer multiplexer, IOptions<RedisConnection> configRedis)
        {
            _multiplexer = multiplexer;
            _configRedis = configRedis.Value;
            connectionRedis = ConnectionMultiplexer.Connect($"{_configRedis.Host}:{_configRedis.Port}");
            clientRedis = connectionRedis.GetDatabase();
        }

        public async Task Delete(string key)
        {
            if (await clientRedis.KeyExistsAsync(key))
                await clientRedis.KeyDeleteAsync(key);
        }

        public List<string> GetAllKeys()
        {
            var keys = new List<string>();

            foreach (var key in _multiplexer.GetServer("127.0.0.1:6379").Keys(pattern: "*"))
                keys.Add(key);

            return keys.Count() > 0 ? keys : null;
        }

        public async Task<List<string>> GetAllKeysWithValue()
        {
            var value = new List<string>();

            foreach (var key in _multiplexer.GetServer("127.0.0.1:6379").Keys(pattern: "*"))
                value.Add(await Get(key));

            return value.Count() > 0 ? value : null;
        }

        public async Task<List<T>> GetAllKeysWithValue<T>() where T : new()
        {
            var value = new List<T>();

            foreach (var key in _multiplexer.GetServer("127.0.0.1:6379").Keys(pattern: "*"))
                value.Add(await GetJson<T>(key));

            return value.Count() > 0 ? value : null;
        }

        public async Task<string> Get(string key)
        {
            var value = await clientRedis.StringGetAsync(key);

            return value.HasValue ? value.ToString() : null;
        }

        public async Task<T> Get<T>(string key)
        {
            var value = await clientRedis.StringGetAsync(key);

            return value.HasValue ? (T)Convert.ChangeType(value, typeof(T)) : default(T);
        }

        public async Task<T> GetJson<T>(string key) where T : new()
        {
            var value = await clientRedis.StringGetAsync(key);

            return value.HasValue ? JsonConvert.DeserializeObject<T>(value) : default(T);
        }

        public async Task<IEnumerable<T>> GetIEnumerableJson<T>(string key)
        {
            var value = await clientRedis.StringGetAsync(key);

            return value.HasValue ? JsonConvert.DeserializeObject<IEnumerable<T>>(value) : default(IEnumerable<T>);
        }

        public async Task Set(string key, string value)
        {
            await clientRedis.StringSetAsync(key, value);
        }

        public void Dispose()
        {
            connectionRedis?.Dispose();
        }
    }
}
