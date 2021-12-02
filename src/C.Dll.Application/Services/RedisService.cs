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

        public async Task<T> Get<T>(string key, int db = 0) where T : class
        {
            if (!this.cacheClient.GetDb(db).Database.IsConnected(key, StackExchange.Redis.CommandFlags.None)) return null;
            return await cacheClient.GetDb(db).GetAsync<T>(key);
        }

        public async Task<bool> Set<T>(string key, T value, int minutes = 2, int db = 0) where T : class
        {
            TimeSpan timespan = new(0, minutes, 0);
            if (!this.cacheClient.GetDb(db).Database.IsConnected(key, StackExchange.Redis.CommandFlags.None)) return false;
            return await cacheClient.GetDb(db).AddAsync<T>(key, value, timespan);
        }

        public async Task<bool> Remove(string key, int db = 0)
        {
             if(!this.cacheClient.GetDb(db).Database.IsConnected(key, StackExchange.Redis.CommandFlags.None)) return false;
            return await cacheClient.GetDb(db).RemoveAsync(key);
        }

        public async Task<bool> UpdateExpiration(string key, DateTimeOffset dateTimeOffSet, int db = 0)
        {
             if(!this.cacheClient.GetDb(db).Database.IsConnected(key, StackExchange.Redis.CommandFlags.None)) return false;
            return await cacheClient.GetDb(db).UpdateExpiryAsync(key, dateTimeOffSet);
        }
    }
}