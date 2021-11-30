using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace Application.Extensions
{
    public static partial class ServiceExtentions
    {
        public static IServiceCollection AddStackExchangeRedis(this IServiceCollection services, IConfiguration _configuration)
        {
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(options =>
            {
                return _configuration.GetSection("Redis").Get<RedisConfiguration>();
            });
            return services;
        }
    }
}