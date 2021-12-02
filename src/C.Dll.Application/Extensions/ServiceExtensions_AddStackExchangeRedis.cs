using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReasonSystems.DLL.SwissKnife;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace Application.Extensions
{
    public static partial class ServiceExtentions
    {
        public static IServiceCollection AddStackExchangeRedis(this IServiceCollection services, IConfiguration _configuration)
        {
            var container = SecretsHandlerService.GetFromEnv("DOTNET_RUNNING_IN_CONTAINER");

            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(options =>
            {
                var redisConfiguration = new RedisConfiguration()
                {
                    AbortOnConnectFail = true,
                    Password = "",
                    AllowAdmin = false,
                    Ssl = false,
                    ConnectTimeout = 2000,
                    Database = 0,
                    Hosts = new RedisHost[]
                    {
                        new RedisHost(){
                            Host = (container=="true") ? "redis" : "localhost",
                            Port = 6379
                         }
                    },
                    ServerEnumerationStrategy = new ServerEnumerationStrategy()
                    {
                        Mode = ServerEnumerationStrategy.ModeOptions.All,
                        TargetRole = ServerEnumerationStrategy.TargetRoleOptions.Any,
                        UnreachableServerAction = ServerEnumerationStrategy.UnreachableServerActionOptions.Throw
                    },
                    MaxValueLength = 1024,
                    PoolSize = 5
                };

                return redisConfiguration;
            });
            return services;
        }
    }
}