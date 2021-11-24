using System;
using Microsoft.Extensions.Configuration;

namespace ReasonSystems.DLL.SwissKnife
{
    public class SecretsHandlerService
    {
        private readonly IConfiguration configuration;

        public SecretsHandlerService(
            IConfiguration configuration
        )
        {
            this.configuration = configuration;
        }

        public string GetFromConfig(string key)
        {
            var section = configuration.GetSection(key);
            return section.Value;
        }

        public static string GetFromEnv(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }

    }
}