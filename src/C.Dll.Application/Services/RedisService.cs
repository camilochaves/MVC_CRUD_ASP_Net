using System;
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

        public async Task<bool> Set<T>(string key, T value, int minutes = 2) where T:class
        {
            TimeSpan timespan = new(0,minutes, 0);
            return await cacheClient.GetDbFromConfiguration().AddAsync<T>(key, value, timespan);
        }

        public async Task<bool> Remove(string key)
        {
            return await cacheClient.GetDbFromConfiguration().RemoveAsync(key);
        }

        public async Task<bool> UpdateExpiration(string key, DateTimeOffset dateTimeOffSet)
        {
            return await cacheClient.GetDbFromConfiguration().UpdateExpiryAsync(key, dateTimeOffSet);
        }
    }
}