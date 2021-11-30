using System.Threading.Tasks;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Application.Services
{
    public class RedisService
    {
        private readonly IRedisCacheClient cacheClient;

        public RedisService(
            IRedisCacheClient cacheClient
        )
        {
            this.cacheClient = cacheClient;
        }

        public async Task<T> Get<T>(string key) where T:class
        {
            return await cacheClient.GetDbFromConfiguration().GetAsync<T>(key);
        }

        public async Task<bool> Set<T>(string key, T value) where T:class
        {
            return await cacheClient.GetDbFromConfiguration().AddAsync<T>(key, value);
        }
    }
}